using Versus.API.DTOs.Responses;
using Versus.API.DTOs.Requests;

namespace Versus.API.Services.Interfaces
{
    public interface ISearchService
    {
        Task<List<SearchResponse>> SearchAsync(SearchRequest request);
    }
}
