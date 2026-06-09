using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.LastFm
{
    internal sealed class LastFmTrack
    {
        public string? Name { get; set; }
        public string? Artist { get; set; }
        public string? Url { get; set; }
        public string? Listeners { get; set; }
        public string? Mbid { get; set; }

        [JsonPropertyName("image")]
        public List<LastFmImage>? Images { get; set; }
    }
}
