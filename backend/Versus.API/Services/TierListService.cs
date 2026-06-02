using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class TierListService(ITierListRepository repo) : ITierListService
    {
        public async Task<PaginatedList<TierListQueryResponse>> GetAllAsync(TierListQueryRequest request)
        {
            return await repo.GetAllAsync(request);
        }

        public async Task<PaginatedList<TierListQueryResponse>> GetBySessionIdAsync(Guid sessionId, TierListQueryRequest request)
        {
            return await repo.GetBySessionIdAsync(sessionId, request);
        }
    }
}
