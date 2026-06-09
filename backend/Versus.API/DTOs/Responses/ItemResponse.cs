namespace Versus.API.DTOs.Responses
{
    public class ItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? ExternalId { get; set; }
        public string? ExternalSource { get; set; }
        public float EloRating { get; set; }
        public int MatchCount { get; set; }
    }
}
