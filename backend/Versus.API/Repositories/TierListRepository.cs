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
                .Where(t => t.Id == id)
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

        public async Task<Guid> CreateAsync(TierList tierList, IEnumerable<Item> items)
        {
            await db.TierList.AddAsync(tierList);
            await db.Items.AddRangeAsync(items);

            await db.SaveChangesAsync();
            return tierList.Id;
        }
    }
}