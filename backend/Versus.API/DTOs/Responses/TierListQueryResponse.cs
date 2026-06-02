namespace Versus.API.DTOs.Responses
{
    public class TierListQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public int MatchCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}