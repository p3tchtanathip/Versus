using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Integrations.Search;
using Versus.API.Services.Interfaces;

namespace Versus.API.Services
{
    public class SearchService(IEnumerable<ISearchProvider> providers) : ISearchService
    {
        public async Task<List<SearchResponse>> SearchAsync(SearchRequest request)
        {
            var provider = providers.FirstOrDefault(
                p => p.CanHandle(request.Category!.Value))
                ?? throw new InvalidOperationException(
                    $"No search provider registered for category '{request.Category}'.");

            request.Query = request.Query.Trim();
            return await provider.SearchAsync(request);
        }
    }
}
