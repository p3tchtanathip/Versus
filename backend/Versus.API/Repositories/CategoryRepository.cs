using Microsoft.EntityFrameworkCore;
using Versus.API.Data;
using Versus.API.DTOs.Responses;
using Versus.API.Repositories.Interfaces;

namespace Versus.API.Repositories
{
    public class CategoryRepository(AppDbContext db) : ICategoryRepository
    {
        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var result = await db.Categories
                .AsNoTracking()
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    IconUrl = c.IconUrl,
                })
                .ToListAsync();

            return result;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return db.Categories.AnyAsync(c => c.Id == id && c.IsActive);
        }
    }
}