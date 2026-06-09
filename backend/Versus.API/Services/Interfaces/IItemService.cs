using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;

namespace Versus.API.Services.Interfaces
{
    public interface IItemService
    {
        Task CreateManyAsync(Guid tierListId, IEnumerable<CreateTierListItemRequest> items);
        Task<ItemResponse> AddAsync(Guid tierListId, CreateTierListItemRequest request);
        Task DeleteAsync(Guid tierListId, Guid itemId);
        Task<ItemEloHistoryResponse> GetEloHistoryAsync(Guid itemId);
    }
}
