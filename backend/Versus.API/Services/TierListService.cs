using Versus.API.Context;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Middleware;
using Versus.API.Models.Common;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class TierListService(ITierListRepository repo, ICurrentSession session) : ITierListService
    {
        public async Task<PaginatedList<TierListResponse>> GetAllAsync(TierListQueryRequest request)
        {
            return await repo.GetAllAsync(request);
        }

        public async Task<PaginatedList<TierListResponse>> GetMeAsync(TierListQueryRequest request)
        {
            if (session.SessionId is not Guid sessionId)
            {
                throw new UnauthorizedAccessException();
            }

            return await repo.GetBySessionIdAsync(sessionId, request);
        }

        public async Task<TierListResponse> GetByIdAsync(Guid id)
        {
            var tierList = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Tier list not found.");
            return tierList;
        }
    }
}
