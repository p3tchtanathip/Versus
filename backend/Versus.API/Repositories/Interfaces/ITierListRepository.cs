using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;

namespace Versus.API.Repositories.Interfaces
{
    public interface ITierListRepository
    {
        Task<PaginatedList<TierListQueryResponse>> GetAllAsync(TierListQueryRequest request);
        Task<PaginatedList<TierListQueryResponse>> GetBySessionIdAsync(Guid sessionId, TierListQueryRequest request);
        // Task<TierList?> GetByIdAsync(Guid id);
        // Task CreateAsync(TierList tierList);
    }
}