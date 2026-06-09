using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models;
using Versus.API.Models.Common;

namespace Versus.API.Repositories.Interfaces
{
    public interface ITierListRepository
    {
        Task<PaginatedList<TierListResponse>> GetAllAsync(TierListQueryRequest request);
        Task<PaginatedList<TierListResponse>> GetBySessionIdAsync(Guid sessionId, TierListQueryRequest request);
        Task<TierListResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(TierList tierList, IEnumerable<Item> items);
    }
}