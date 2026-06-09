using System.Net.Http.Headers;
using Versus.API.Constants;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Enums;

namespace Versus.API.Integrations.Search.Tmdb
{
    public class TmdbSearchProvider(HttpClient httpClient, IConfiguration configuration) : ISearchProvider
    {
        private const string SearchBaseUrl = "https://api.themoviedb.org/3/search";
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        public bool CanHandle(TierListCategory category)
        {
            return category is TierListCategory.Movie or TierListCategory.Series;
        }

        public async Task<List<SearchResponse>> SearchAsync(SearchRequest request)
        {
            var mediaType = request.Category == TierListCategory.Movie ? "movie" : "tv";
            var token = configuration["TMDB:ReadAccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("TMDB API key is not configured");
            }

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"{SearchBaseUrl}/{mediaType}?query={Uri.EscapeDataString(request.Query)}&include_adult=false&language=en-US&page=1");

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await SendAsync<TmdbSearchResponse>(httpRequest);
            return response.Results
                .Where(item => item.Id is not null)
                .Select(item => Map(item, mediaType))
                .Where(item => !string.IsNullOrWhiteSpace(item.Name))
                .ToList();
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage request)
            where T : new()
        {
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(SearchProviderJson.Options) ?? new T();
        }

        private static SearchResponse Map(TmdbSearchItem item, string mediaType)
        {
            return new SearchResponse
            {
                ExternalId = item.Id?.ToString(),
                ExternalSource = ExternalSource.Tmdb,
                Name = mediaType == "movie" ? item.Title : item.Name,
                ImageUrl = BuildImageUrl(item.PosterPath),
                Metadata = SearchProviderJson.SerializeMetadata(new
                {
                    type = mediaType == "movie" ? "movie" : "series",
                    overview = item.Overview,
                    releaseDate = mediaType == "movie" ? item.ReleaseDate : item.FirstAirDate,
                    voteAverage = item.VoteAverage
                })
            };
        }

        private static string? BuildImageUrl(string? path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : $"{ImageBaseUrl}{path}";
        }
    }
}
