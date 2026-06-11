using Microsoft.EntityFrameworkCore;
using Versus.API.Data;
using Versus.API.DTOs.Responses;
using Versus.API.Models;
using Versus.API.Repositories.Interfaces;

namespace Versus.API.Repositories
{
    public class ItemRepository(AppDbContext db) : IItemRepository
    {
        public Task<int> GetCountAsync(Guid tierListId)
        {
            return db.Items.CountAsync(i => i.TierListId == tierListId);
        }

        public Task<bool> ExistsByExternalIdAsync(Guid tierListId, string externalId)
        {
            return db.Items.AnyAsync(i => i.TierListId == tierListId && i.ExternalId == externalId);
        }

        public Task<Item?> FindAsync(Guid tierListId, Guid itemId)
        {
            return db.Items.FirstOrDefaultAsync(i => i.TierListId == tierListId && i.Id == itemId);
        }

        public async Task<ItemResponse> AddAsync(Item item)
        {
            await db.Items.AddAsync(item);
            await db.SaveChangesAsync();

            return new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                ImageUrl = item.ImageUrl,
                ExternalId = item.ExternalId,
                ExternalSource = item.ExternalSource,
                EloRating = item.EloRating,
                MatchCount = item.MatchCount
            };
        }

        public async Task AddRangeAsync(IEnumerable<Item> items)
        {
            await db.Items.AddRangeAsync(items);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid tierListId, Guid itemId)
        {
            await db.Items
                .Where(i => i.TierListId == tierListId && i.Id == itemId)
                .ExecuteDeleteAsync();
        }

        public Task<List<Item>> GetByTierListIdAsync(Guid tierListId)
        {
            return db.Items
                .AsNoTracking()
                .Where(i => i.TierListId == tierListId)
                .OrderByDescending(i => i.EloRating)
                .ToListAsync();
        }

        public Task<Item?> FindByIdAsync(Guid itemId)
        {
            return db.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task ReloadAsync(Item item)
        {
            await db.Entry(item).ReloadAsync();
        }

        public Task<List<EloHistory>> GetEloHistoryByItemIdAsync(Guid itemId)
        {
            return db.EloHistories
                .AsNoTracking()
                .Include(e => e.Match)
                .Where(e => e.ItemId == itemId)
                .OrderBy(e => e.Match.PlayedAt)
                .ToListAsync();
        }
    }
}
