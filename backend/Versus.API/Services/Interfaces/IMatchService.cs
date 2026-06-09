using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;

namespace Versus.API.Services.Interfaces
{
    public interface IMatchService
    {
        Task<NextMatchResponse> GetNextMatchAsync(Guid tierListId);
        Task<MatchResponse> CreateAsync(CreateMatchRequest request);
        Task<List<MatchResponse>> GetHistoryAsync(Guid tierListId, int limit = 10);
    }
}
