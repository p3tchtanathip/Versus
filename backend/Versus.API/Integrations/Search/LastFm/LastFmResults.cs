using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.LastFm
{
    internal sealed class LastFmResults
    {
        [JsonPropertyName("trackmatches")]
        public LastFmTrackMatches? TrackMatches { get; set; }
    }
}
