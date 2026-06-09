using Versus.API.Models;

namespace Versus.API.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<HashSet<(Guid, Guid)>> GetSessionPairHistoryAsync(Guid tierListId, Guid sessionId);
        Task<List<Match>> GetRecentByTierListIdAsync(Guid tierListId, int limit);
        Task<Match> CreateAsync(
            Match match,
            EloHistory winnerHistory,
            EloHistory loserHistory,
            Item winner,
            Item loser);

        Task<int> CountSessionMatchesAsync(Guid tierListId, Guid sessionId);
    }
}
