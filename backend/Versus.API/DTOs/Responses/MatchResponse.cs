namespace Versus.API.DTOs.Responses
{
    public class MatchResponse
    {
        public Guid Id { get; set; }
        public Guid TierListId { get; set; }
        public ItemResponse Winner { get; set; } = null!;
        public ItemResponse Loser { get; set; } = null!;
        public float WinnerDelta { get; set; }
        public float LoserDelta { get; set; }
        public DateTimeOffset PlayedAt { get; set; }
    }
}
