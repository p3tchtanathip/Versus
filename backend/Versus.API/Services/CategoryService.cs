using Versus.API.DTOs.Responses;
using Versus.API.Repositories.Interfaces;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class CategoryService(ICategoryRepository repo) : ICategoryService
    {
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            return await repo.GetAllAsync();
        }
    }
}