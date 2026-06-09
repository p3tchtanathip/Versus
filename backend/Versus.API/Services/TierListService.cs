using Versus.API.Context;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Middleware;
using Versus.API.Models;
using Versus.API.Models.Common;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class TierListService(
        ITierListRepository repo,
        ICategoryRepository categoryRepo,
        IItemRepository itemRepo,
        IItemService itemService,
        ICurrentSession session) : ITierListService
    {
        private static readonly string[] TierOrder = ["S", "A", "B", "C", "D"];
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

        public async Task<TierListResponse> CreateAsync(CreateTierListRequest request)
        {
            if (session.SessionId is not Guid sessionId)
            {
                throw new UnauthorizedAccessException();
            }

            if (!await categoryRepo.ExistsAsync(request.CategoryId))
            {
                throw new NotFoundException("Category not found.");
            }

            var tierList = new TierList
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                CreatorSessionId = sessionId
            };

            var id = await repo.CreateAsync(tierList);
            await itemService.CreateManyAsync(id, request.Items);

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (session.SessionId is not Guid sessionId)
            {
                throw new UnauthorizedAccessException();
            }

            var tierList = await repo.FindByIdAsync(id) ?? throw new NotFoundException("Tier list not found.");

            if (tierList.CreatorSessionId != sessionId)
            {
                throw new ForbiddenAccessException("You can only delete tier lists you created.");
            }

            await repo.DeleteAsync(id);
        }

        public async Task<TierListResultsResponse> GetResultsAsync(Guid id)
        {
            _ = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Tier list not found.");

            var items = await itemRepo.GetByTierListIdAsync(id);
            var grouped = items
                .Select(item => new TierResultItemResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImageUrl = item.ImageUrl,
                    ExternalId = item.ExternalId,
                    ExternalSource = item.ExternalSource,
                    EloRating = item.EloRating,
                    MatchCount = item.MatchCount
                })
                .GroupBy(i => i.TierLabel)
                .ToDictionary(g => g.Key, g => g.ToList());

            return new TierListResultsResponse
            {
                Tiers = TierOrder
                    .Where(grouped.ContainsKey)
                    .Select(label => new TierGroupResponse
                    {
                        TierLabel = label,
                        Items = grouped[label]
                    })
                    .ToList()
            };
        }
    }
}
