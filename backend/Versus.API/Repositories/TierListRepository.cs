using Microsoft.EntityFrameworkCore;
using Versus.API.Data;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models;
using Versus.API.Models.Common;
using Versus.API.Repositories.Interfaces;

namespace Versus.API.Repositories
{
    public class TierListRepository(AppDbContext db) : ITierListRepository
    {
        public async Task<PaginatedList<TierListResponse>> GetAllAsync(TierListQueryRequest request)
        {
            var query = db.TierList
                .AsNoTracking()
                .Where(t =>
                    t.DeletedAt == null &&
                    (string.IsNullOrWhiteSpace(request.Search) || t.Name.Contains(request.Search)) &&
                    (!request.CategoryId.HasValue || t.CategoryId == request.CategoryId.Value)
                )
                .OrderByDescending(t => t.CreatedAt);

            var count = await query.CountAsync();

            var result = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TierListResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    ItemCount = t.Items.Count(),
                    MatchCount = t.Matches.Count(),
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PaginatedList<TierListResponse>(
                result,
                count,
                request.PageNumber,
                request.PageSize
            );
        }

        public async Task<PaginatedList<TierListResponse>> GetBySessionIdAsync(Guid sessionId, TierListQueryRequest request)
        {
            var query = db.TierList
                .AsNoTracking()
                .Where(t =>
                    t.DeletedAt == null &&
                    t.CreatorSessionId == sessionId &&
                    (string.IsNullOrWhiteSpace(request.Search) || t.Name.Contains(request.Search)) &&
                    (!request.CategoryId.HasValue || t.CategoryId == request.CategoryId.Value)
                )
                .OrderByDescending(t => t.CreatedAt);

            var count = await query.CountAsync();

            var result = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TierListResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    ItemCount = t.Items.Count(),
                    MatchCount = t.Matches.Count(),
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PaginatedList<TierListResponse>(
                result,
                count,
                request.PageNumber,
                request.PageSize
            );
        }

        public async Task<TierListResponse?> GetByIdAsync(Guid id)
        {
            return await db.TierList
                .AsNoTracking()
                .Where(t => t.Id == id && t.DeletedAt == null)
                .Select(t => new TierListResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    ItemCount = t.Items.Count(),
                    MatchCount = t.Matches.Count(),
                    CreatedAt = t.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TierList?> FindByIdAsync(Guid id)
        {
            return await db.TierList
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);
        }

        public async Task<Guid> CreateAsync(TierList tierList)
        {
            await db.TierList.AddAsync(tierList);
            await db.SaveChangesAsync();
            return tierList.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await db.TierList
                .Where(t => t.Id == id && t.DeletedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.DeletedAt, DateTime.UtcNow));
        }
    }
}