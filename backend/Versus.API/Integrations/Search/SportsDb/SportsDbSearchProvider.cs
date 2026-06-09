using Versus.API.Constants;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Enums;

namespace Versus.API.Integrations.Search.SportsDb
{
    public class SportsDbSearchProvider(HttpClient httpClient, IConfiguration configuration) : ISearchProvider
    {
        private const string BaseUrl = "https://www.thesportsdb.com/api/v1/json";

        public bool CanHandle(TierListCategory category)
        {
            return category == TierListCategory.Sport;
        }

        public async Task<List<SearchResponse>> SearchAsync(SearchRequest request)
        {
            var apiKey = configuration["TheSportsDB:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("TheSportsDB API key is not configured");
            }

            var type = request.Type ?? SportsSearchType.All;
            var results = new List<SearchResponse>();
            var teams = new List<SportsDbTeam>();

            if (type is SportsSearchType.All or SportsSearchType.Team)
            {
                teams = await SearchTeamsAsync(apiKey, request.Query);
                results.AddRange(teams.Select(MapTeam));
            }

            if (type is SportsSearchType.All or SportsSearchType.Player)
            {
                var players = await SearchPlayersAsync(apiKey, request.Query);
                results.AddRange(players.Select(player => MapPlayer(player)));
            }

            if (type == SportsSearchType.All)
            {
                foreach (var team in teams.Where(team => !string.IsNullOrWhiteSpace(team.IdTeam)))
                {
                    var teamPlayers = await LookupTeamPlayersAsync(apiKey, team.IdTeam!);
                    results.AddRange(teamPlayers.Select(player => MapPlayer(player, team.StrTeam)));
                }
            }

            return results
                .Where(item => !string.IsNullOrWhiteSpace(item.Name))
                .GroupBy(item => $"{item.ExternalSource}:{item.ExternalId}:{item.Name}")
                .Select(group => group.First())
                .ToList();
        }

        private async Task<List<SportsDbTeam>> SearchTeamsAsync(string apiKey, string query)
        {
            var url = $"{BuildApiRoot(apiKey)}/searchteams.php?t={Uri.EscapeDataString(query)}";
            var response = await SendAsync<SportsDbTeamSearchResponse>(url);
            return response.Teams ?? [];
        }

        private async Task<List<SportsDbPlayer>> SearchPlayersAsync(string apiKey, string query)
        {
            var url = $"{BuildApiRoot(apiKey)}/searchplayers.php?p={Uri.EscapeDataString(query)}";
            var response = await SendAsync<SportsDbPlayerSearchResponse>(url);
            return response.Players ?? [];
        }

        private async Task<List<SportsDbPlayer>> LookupTeamPlayersAsync(string apiKey, string teamId)
        {
            var url = $"{BuildApiRoot(apiKey)}/lookup_all_players.php?id={Uri.EscapeDataString(teamId)}";
            var response = await SendAsync<SportsDbPlayerSearchResponse>(url);
            return response.Players ?? [];
        }

        private async Task<T> SendAsync<T>(string url)
            where T : new()
        {
            return await httpClient.GetFromJsonAsync<T>(url, SearchProviderJson.Options) ?? new T();
        }

        private static string BuildApiRoot(string apiKey)
        {
            return $"{BaseUrl}/{Uri.EscapeDataString(apiKey)}";
        }

        private static SearchResponse MapTeam(SportsDbTeam team)
        {
            return new SearchResponse
            {
                ExternalId = team.IdTeam,
                ExternalSource = ExternalSource.SportsDb,
                Name = team.StrTeam,
                ImageUrl = SearchProviderText.FirstNotEmpty(team.StrTeamBadge, team.StrTeamLogo, team.StrTeamFanart1),
                Metadata = SearchProviderJson.SerializeMetadata(new
                {
                    type = "team",
                    sport = team.StrSport,
                    league = team.StrLeague,
                    country = team.StrCountry,
                    stadium = team.StrStadium
                })
            };
        }

        private static SearchResponse MapPlayer(SportsDbPlayer player, string? teamName = null)
        {
            return new SearchResponse
            {
                ExternalId = player.IdPlayer,
                ExternalSource = ExternalSource.SportsDb,
                Name = player.StrPlayer,
                ImageUrl = SearchProviderText.FirstNotEmpty(player.StrCutout, player.StrThumb, player.StrRender),
                Metadata = SearchProviderJson.SerializeMetadata(new
                {
                    type = "player",
                    team = SearchProviderText.FirstNotEmpty(player.StrTeam, teamName),
                    sport = player.StrSport,
                    nationality = player.StrNationality,
                    position = player.StrPosition
                })
            };
        }
    }
}
