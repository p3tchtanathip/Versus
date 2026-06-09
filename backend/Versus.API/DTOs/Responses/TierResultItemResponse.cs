namespace Versus.API.DTOs.Responses
{
    public class TierResultItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? ExternalId { get; set; }
        public string? ExternalSource { get; set; }
        public float EloRating { get; set; }
        public int MatchCount { get; set; }

        public string TierLabel => EloRating switch
        {
            >= 1200 => "S",
            >= 1100 => "A",
            >= 1000 => "B",
            >= 900 => "C",
            _ => "D"
        };
    }
}
