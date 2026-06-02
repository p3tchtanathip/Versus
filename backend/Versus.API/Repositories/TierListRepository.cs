using Microsoft.EntityFrameworkCore;
using Versus.API.Data;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;
using Versus.API.Repositories.Interfaces;

namespace Versus.API.Repositories
{
    public class TierListRepository(AppDbContext db) : ITierListRepository
    {
        public async Task<PaginatedList<TierListQueryResponse>> GetAllAsync(TierListQueryRequest request)
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
                .Select(t => new TierListQueryResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    ItemCount = t.Items.Count(),
                    MatchCount = t.Matches.Count(),
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PaginatedList<TierListQueryResponse>(
                result,
                count,
                request.PageNumber,
                request.PageSize
            );
        }

        public async Task<PaginatedList<TierListQueryResponse>> GetBySessionIdAsync(Guid sessionId, TierListQueryRequest request)
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
                .Select(t => new TierListQueryResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name,
                    ItemCount = t.Items.Count(),
                    MatchCount = t.Matches.Count(),
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PaginatedList<TierListQueryResponse>(
                result,
                count,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}