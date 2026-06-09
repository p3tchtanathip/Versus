using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;

namespace Versus.API.Services.Interfaces
{
    public interface ITierListService
    {
        Task<PaginatedList<TierListResponse>> GetAllAsync(TierListQueryRequest request);
        Task<PaginatedList<TierListResponse>> GetMeAsync(TierListQueryRequest request);
        Task<TierListResponse> GetByIdAsync(Guid id);
        Task<TierListResponse> CreateAsync(CreateTierListRequest request);
        Task DeleteAsync(Guid id);
        Task<TierListResultsResponse> GetResultsAsync(Guid id);
    }
}