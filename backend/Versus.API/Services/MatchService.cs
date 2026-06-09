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

            var pairHistory = await matchRepo.GetSessionPairHistoryAsync(tierListId, sessionId);
            var (itemA, itemB) = pairingService.SelectPair(items, pairHistory);

            return new NextMatchResponse
            {
                ItemA = MapItem(itemA),
                ItemB = MapItem(itemB)
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

            var winner = await itemRepo.FindAsync(request.TierListId, request.WinnerId)
                ?? throw new NotFoundException("Winner item not found.");

            var loser = await itemRepo.FindAsync(request.TierListId, request.LoserId)
                ?? throw new NotFoundException("Loser item not found.");

            var eloResult = eloService.Calculate(winner.EloRating, loser.EloRating);

            var match = new Match
            {
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
