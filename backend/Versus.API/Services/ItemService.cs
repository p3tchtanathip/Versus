using Versus.API.Context;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Middleware;
using Versus.API.Models;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class ItemService(
        IItemRepository itemRepo,
        ITierListRepository tierListRepo,
        ICurrentSession session) : IItemService
    {
        public async Task CreateManyAsync(Guid tierListId, IEnumerable<CreateTierListItemRequest> items)
        {
            var itemList = items.ToList();
            ValidateItemCollection(itemList);

            var entities = itemList.Select(item => new Item
            {
                TierListId = tierListId,
                Name = item.Name,
                ImageUrl = item.ImageUrl,
                ExternalId = item.ExternalId,
                ExternalSource = item.ExternalSource
            });

            await itemRepo.AddRangeAsync(entities);
        }

        public async Task<ItemResponse> AddAsync(Guid tierListId, CreateTierListItemRequest request)
        {
            await EnsureCreatorAsync(tierListId);

            var itemCount = await itemRepo.GetCountAsync(tierListId);
            if (itemCount >= 50)
            {
                throw new ArgumentException("Tier list cannot exceed 50 items.");
            }

            if (!string.IsNullOrWhiteSpace(request.ExternalId) &&
                await itemRepo.ExistsByExternalIdAsync(tierListId, request.ExternalId))
            {
                throw new ConflictException("Item already exists in this tier list.");
            }

            var item = new Item
            {
                TierListId = tierListId,
                Name = request.Name,
                ImageUrl = request.ImageUrl,
                ExternalId = request.ExternalId,
                ExternalSource = request.ExternalSource
            };

            return await itemRepo.AddAsync(item);
        }

        public async Task DeleteAsync(Guid tierListId, Guid itemId)
        {
            await EnsureCreatorAsync(tierListId);

            var item = await itemRepo.FindAsync(tierListId, itemId) ?? throw new NotFoundException("Item not found.");

            var itemCount = await itemRepo.GetCountAsync(tierListId);
            if (itemCount <= 2)
            {
                throw new ArgumentException("Tier list must have at least 2 items.");
            }

            if (item.MatchCount > 0)
            {
                throw new ConflictException("Cannot delete an item that has match history.");
            }

            await itemRepo.DeleteAsync(tierListId, itemId);
        }

        public async Task<ItemEloHistoryResponse> GetEloHistoryAsync(Guid itemId)
        {
            var item = await itemRepo.FindByIdAsync(itemId) ?? throw new NotFoundException("Item not found.");

            var histories = await itemRepo.GetEloHistoryByItemIdAsync(itemId);

            return new ItemEloHistoryResponse
            {
                ItemId = item.Id,
                ItemName = item.Name,
                InitialRating = histories.Count > 0 ? histories[0].RatingBefore : item.EloRating,
                Points = histories.Select(h => new EloHistoryGraphPointResponse
                {
                    PlayedAt = h.Match.PlayedAt,
                    RatingBefore = h.RatingBefore,
                    RatingAfter = h.RatingAfter,
                    Delta = h.Delta,
                    MatchId = h.MatchId
                }).ToList()
            };
        }

        private async Task EnsureCreatorAsync(Guid tierListId)
        {
            if (session.SessionId is not Guid sessionId)
            {
                throw new UnauthorizedAccessException();
            }

            var tierList = await tierListRepo.FindByIdAsync(tierListId) ?? throw new NotFoundException("Tier list not found.");

            if (tierList.CreatorSessionId != sessionId)
            {
                throw new ForbiddenAccessException("You can only modify tier lists you created.");
            }
        }

        private static void ValidateItemCollection(List<CreateTierListItemRequest> items)
        {
            if (items.Count < 2)
            {
                throw new ArgumentException("Tier list must have at least 2 items");
            }

            if (items.Count > 50)
            {
                throw new ArgumentException("Tier list cannot exceed 50 items");
            }

            var hasDuplicates = items
                .Where(i => !string.IsNullOrWhiteSpace(i.ExternalId))
                .GroupBy(i => i.ExternalId)
                .Any(g => g.Count() > 1);

            if (hasDuplicates)
            {
                throw new ArgumentException("Tier list items must be unique.");
            }
        }
    }
}
