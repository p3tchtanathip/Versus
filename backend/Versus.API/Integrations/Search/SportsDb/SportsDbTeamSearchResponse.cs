using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.SportsDb
{
    internal sealed class SportsDbTeamSearchResponse
    {
        [JsonPropertyName("teams")]
        public List<SportsDbTeam>? Teams { get; set; }
    }
}
