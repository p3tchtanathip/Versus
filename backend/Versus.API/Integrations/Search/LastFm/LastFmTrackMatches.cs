using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.LastFm
{
    internal sealed class LastFmTrackMatches
    {
        [JsonPropertyName("track")]
        public List<LastFmTrack>? Tracks { get; set; }
    }
}
