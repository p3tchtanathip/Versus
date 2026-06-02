using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;

namespace Versus.API.Services.Interfaces
{
    public interface ITierListService
    {
        Task<PaginatedList<TierListQueryResponse>> GetAllAsync(TierListQueryRequest request);
        Task<PaginatedList<TierListQueryResponse>> GetBySessionIdAsync(Guid sessionId, TierListQueryRequest request);
    }
}