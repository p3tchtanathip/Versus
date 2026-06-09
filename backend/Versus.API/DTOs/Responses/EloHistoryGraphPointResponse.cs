namespace Versus.API.DTOs.Responses
{
    public class EloHistoryGraphPointResponse
    {
        public DateTime PlayedAt { get; set; }
        public float RatingBefore { get; set; }
        public float RatingAfter { get; set; }
        public float Delta { get; set; }
        public Guid MatchId { get; set; }
    }
}
