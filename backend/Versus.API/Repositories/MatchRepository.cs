using Microsoft.EntityFrameworkCore;
using Versus.API.Data;
using Versus.API.Models;
using Versus.API.Repositories.Interfaces;

namespace Versus.API.Repositories
{
    public class MatchRepository(AppDbContext db) : IMatchRepository
    {
        public async Task<HashSet<(Guid, Guid)>> GetSessionPairHistoryAsync(Guid tierListId, Guid sessionId)
        {
            var matches = await db.Matches
                .AsNoTracking()
                .Where(m => m.TierListId == tierListId && m.SessionId == sessionId && !m.IsPlayAgain)
                .Select(m => new { m.WinnerId, m.LoserId })
                .ToListAsync();

            return matches
                .Select(m => NormalizePairKey(m.WinnerId, m.LoserId))
                .ToHashSet();
        }

        public Task<List<Match>> GetRecentByTierListIdAsync(Guid tierListId, int limit)
        {
            return db.Matches
                .AsNoTracking()
                .Include(m => m.Winner)
                .Include(m => m.Loser)
                .Include(m => m.EloHistories)
                .Where(m => m.TierListId == tierListId)
                .OrderByDescending(m => m.PlayedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Match> CreateAsync(
            Match match,
            EloHistory winnerHistory,
            EloHistory loserHistory,
            Item winner,
            Item loser)
        {
            winner.EloRating = winnerHistory.RatingAfter;
            winner.MatchCount += 1;
            loser.EloRating = loserHistory.RatingAfter;
            loser.MatchCount += 1;

            await db.Matches.AddAsync(match);
            await db.EloHistories.AddRangeAsync(winnerHistory, loserHistory);
            db.Items.Update(winner);
            db.Items.Update(loser);

            await db.SaveChangesAsync();
            return match;
        }

        public async Task<int> CountSessionMatchesAsync(Guid tierListId, Guid sessionId)
        {
            return await db.Matches
                .CountAsync(m => m.TierListId == tierListId && m.SessionId == sessionId && !m.IsPlayAgain);
        }

        public async Task ArchiveSessionMatchesAsync(Guid tierListId, Guid sessionId)
        {
            await db.Matches
                .Where(m => m.TierListId == tierListId && m.SessionId == sessionId && !m.IsPlayAgain)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.IsPlayAgain, true));
        }

        private static (Guid, Guid) NormalizePairKey(Guid idA, Guid idB)
        {
            return idA.CompareTo(idB) < 0 ? (idA, idB) : (idB, idA);
        }
    }
}
