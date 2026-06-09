using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.SportsDb
{
    internal sealed class SportsDbPlayerSearchResponse
    {
        [JsonPropertyName("player")]
        public List<SportsDbPlayer>? Players { get; set; }
    }
}
