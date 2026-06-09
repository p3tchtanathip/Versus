namespace Versus.API.DTOs.Responses
{
    public class TierListResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public int ItemCount { get; set; }
        public int MatchCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}