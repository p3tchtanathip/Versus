using Microsoft.EntityFrameworkCore;
using Versus.API.Context;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Middleware;
using Versus.API.Models;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class MatchService(
        ITierListRepository tierListRepo,
        IItemRepository itemRepo,
        IMatchRepository matchRepo,
        IPairingService pairingService,
        IEloService eloService,
        ICurrentSession session) : IMatchService
    {
        public async Task<NextMatchResponse> GetNextMatchAsync(Guid tierListId)
        {
            var sessionId = RequireSession();

            _ = await tierListRepo.FindByIdAsync(tierListId)
                ?? throw new NotFoundException("Tier list not found.");

            var items = await itemRepo.GetByTierListIdAsync(tierListId);
            if (items.Count < 2)
            {
                throw new ArgumentException("Tier list must have at least 2 items to match.");
            }

            var totalPairs = items.Count * (items.Count - 1) / 2;
            var playedCount = await matchRepo.CountSessionMatchesAsync(tierListId, sessionId);

            var pairHistory = await matchRepo.GetSessionPairHistoryAsync(tierListId, sessionId);

            var pair = pairingService.SelectPair(items, pairHistory);

            if (pair is null)
            {
                return new NextMatchResponse
                {
                    Match = null,
                    Progress = new ProgressDto { Played = playedCount, Total = totalPairs }
                };
            }

            var (itemA, itemB) = pair.Value;

            return new NextMatchResponse
            {
                Match = itemA != null && itemB != null
                    ? new MatchPairDto { ItemA = MapItem(itemA), ItemB = MapItem(itemB) }
                    : null,
                Progress = new ProgressDto
                {
                    Played = playedCount,
                    Total = totalPairs
                }
            };
        }

        public async Task<MatchResponse> CreateAsync(CreateMatchRequest request)
        {
            var sessionId = RequireSession();

            _ = await tierListRepo.FindByIdAsync(request.TierListId)
                ?? throw new NotFoundException("Tier list not found.");

            if (request.WinnerId == request.LoserId)
            {
                throw new ArgumentException("Winner and loser must be different items.");
            }

            const int maxRetries = 1;

            for (var attempt = 0; attempt <= maxRetries; attempt++)
            {
                var winner = await itemRepo.FindAsync(request.TierListId, request.WinnerId)
                    ?? throw new NotFoundException("Winner item not found.");

                var loser = await itemRepo.FindAsync(request.TierListId, request.LoserId)
                    ?? throw new NotFoundException("Loser item not found.");

                if (winner.EloRating <= 0) winner.EloRating = EloService.DefaultRating;
                if (loser.EloRating <= 0) loser.EloRating = EloService.DefaultRating;

                var eloResult = eloService.Calculate(winner.EloRating, loser.EloRating);

                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    TierListId = request.TierListId,
                    SessionId = sessionId,
                    WinnerId = winner.Id,
                    LoserId = loser.Id
                };

                var winnerHistory = new EloHistory
                {
                    Id = Guid.NewGuid(),
                    MatchId = match.Id,
                    ItemId = winner.Id,
                    RatingBefore = eloResult.WinnerRatingBefore,
                    RatingAfter = eloResult.WinnerRatingAfter,
                    Delta = eloResult.WinnerDelta
                };

                var loserHistory = new EloHistory
                {
                    Id = Guid.NewGuid(),
                    MatchId = match.Id,
                    ItemId = loser.Id,
                    RatingBefore = eloResult.LoserRatingBefore,
                    RatingAfter = eloResult.LoserRatingAfter,
                    Delta = eloResult.LoserDelta
                };

                try
                {
                    var savedMatch = await matchRepo.CreateAsync(match, winnerHistory, loserHistory, winner, loser);

                    return new MatchResponse
                    {
                        Id = savedMatch.Id,
                        TierListId = savedMatch.TierListId,
                        Winner = MapItem(winner),
                        Loser = MapItem(loser),
                        WinnerDelta = eloResult.WinnerDelta,
                        LoserDelta = eloResult.LoserDelta,
                        PlayedAt = savedMatch.PlayedAt
                    };
                }
                catch (DbUpdateConcurrencyException) when (attempt < maxRetries)
                {
                    await itemRepo.ReloadAsync(winner);
                    await itemRepo.ReloadAsync(loser);
                }
            }

            throw new InvalidOperationException("Failed to save match due to concurrent modification.");
        }

        public async Task ResetSessionHistoryAsync(Guid tierListId)
        {
            var sessionId = RequireSession();

            _ = await tierListRepo.FindByIdAsync(tierListId)
                ?? throw new NotFoundException("Tier list not found.");

            await matchRepo.ArchiveSessionMatchesAsync(tierListId, sessionId);
        }

        public async Task<List<MatchResponse>> GetHistoryAsync(Guid tierListId, int limit = 10)
        {
            _ = await tierListRepo.FindByIdAsync(tierListId)
                ?? throw new NotFoundException("Tier list not found.");

            var matches = await matchRepo.GetRecentByTierListIdAsync(tierListId, limit);
            return matches.Select(MapMatch).ToList();
        }

        private Guid RequireSession()
        {
            return session.SessionId ?? throw new UnauthorizedAccessException();
        }

        private static ItemResponse MapItem(Item item)
        {
            return new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                ImageUrl = item.ImageUrl,
                ExternalId = item.ExternalId,
                ExternalSource = item.ExternalSource,
                EloRating = item.EloRating,
                MatchCount = item.MatchCount
            };
        }

        private static MatchResponse MapMatch(Match match)
        {
            var winnerHistory = match.EloHistories.First(e => e.ItemId == match.WinnerId);
            var loserHistory = match.EloHistories.First(e => e.ItemId == match.LoserId);

            return new MatchResponse
            {
                Id = match.Id,
                TierListId = match.TierListId,
                Winner = MapItem(match.Winner),
                Loser = MapItem(match.Loser),
                WinnerDelta = winnerHistory.Delta,
                LoserDelta = loserHistory.Delta,
                PlayedAt = match.PlayedAt
            };
        }
    }
}
