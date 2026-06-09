using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.LastFm
{
    internal sealed class LastFmImage
    {
        [JsonPropertyName("#text")]
        public string? Url { get; set; }
    }
}
