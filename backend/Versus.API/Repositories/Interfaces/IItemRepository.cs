using Versus.API.DTOs.Responses;
using Versus.API.Models;

namespace Versus.API.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<int> GetCountAsync(Guid tierListId);
        Task<bool> ExistsByExternalIdAsync(Guid tierListId, string externalId);
        Task<Item?> FindAsync(Guid tierListId, Guid itemId);
        Task<ItemResponse> AddAsync(Item item);
        Task AddRangeAsync(IEnumerable<Item> items);
        Task DeleteAsync(Guid tierListId, Guid itemId);
        Task<List<Item>> GetByTierListIdAsync(Guid tierListId);
        Task<Item?> FindByIdAsync(Guid itemId);
        Task<List<EloHistory>> GetEloHistoryByItemIdAsync(Guid itemId);
        Task ReloadAsync(Item item);
    }
}
