using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Enums;

namespace Versus.API.Integrations.Search
{
    public interface ISearchProvider
    {
        bool CanHandle(TierListCategory category);
        Task<List<SearchResponse>> SearchAsync(SearchRequest request);
    }
}
