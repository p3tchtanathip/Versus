namespace Versus.API.DTOs.Responses
{
    public class TierListResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public int ItemCount { get; set; }
        public int MatchCount { get; set; }
        public int PlayedCount { get; set; }
        public int TotalPairs { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}