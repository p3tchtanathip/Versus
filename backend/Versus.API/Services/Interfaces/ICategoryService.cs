using Versus.API.DTOs.Responses;

namespace Versus.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync();
    }
}