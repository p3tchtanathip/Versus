using Versus.API.Constants;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Enums;

namespace Versus.API.Integrations.Search.LastFm
{
    public class LastFmSearchProvider(HttpClient httpClient, IConfiguration configuration) : ISearchProvider
    {
        private const string BaseUrl = "https://ws.audioscrobbler.com/2.0/";

        public bool CanHandle(TierListCategory category)
        {
            return category == TierListCategory.Music;
        }

        public async Task<List<SearchResponse>> SearchAsync(SearchRequest request)
        {
            var apiKey = configuration["LastFm:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("LastFm API key is not configured");
            }

            var url =
                $"{BaseUrl}?method=track.search&track={Uri.EscapeDataString(request.Query)}&api_key={Uri.EscapeDataString(apiKey)}&format=json&limit=10";

            var response = await SendAsync<LastFmSearchResponse>(url);
            return response.Results?.TrackMatches?.Tracks?
                .Select(Map)
                .Where(item => !string.IsNullOrWhiteSpace(item.Name))
                .ToList() ?? [];
        }

        private async Task<T> SendAsync<T>(string url)
            where T : new()
        {
            return await httpClient.GetFromJsonAsync<T>(url, SearchProviderJson.Options) ?? new T();
        }

        private static SearchResponse Map(LastFmTrack track)
        {
            return new SearchResponse
            {
                ExternalId = track.Mbid,
                ExternalSource = ExternalSource.LastFm,
                Name = string.IsNullOrWhiteSpace(track.Artist) ? track.Name : $"{track.Name} - {track.Artist}",
                ImageUrl = GetImageUrl(track.Images),
                Metadata = SearchProviderJson.SerializeMetadata(new
                {
                    type = "track",
                    track = track.Name,
                    artist = track.Artist,
                    listeners = track.Listeners,
                    url = track.Url
                })
            };
        }

        private static string? GetImageUrl(List<LastFmImage>? images)
        {
            return images?
                .LastOrDefault(image => !string.IsNullOrWhiteSpace(image.Url))
                ?.Url;
        }
    }
}
