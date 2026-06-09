using Versus.API.DTOs.Responses;

namespace Versus.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResponse>> GetAllAsync();
        Task<bool> ExistsAsync(int id);
    }
}