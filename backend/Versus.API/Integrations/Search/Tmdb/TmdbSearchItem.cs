using System.Text.Json.Serialization;

namespace Versus.API.Integrations.Search.Tmdb
{
    internal sealed class TmdbSearchItem
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Overview { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("first_air_date")]
        public string? FirstAirDate { get; set; }

        [JsonPropertyName("vote_average")]
        public decimal? VoteAverage { get; set; }
    }
}
