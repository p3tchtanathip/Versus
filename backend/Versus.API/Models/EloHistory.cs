namespace Versus.API.Models
{
    public class EloHistory
    {
        public Guid Id { get; set; }
        public float RatingBefore { get; set; }
        public float RatingAfter { get; set; }
        public float Delta { get; set; }

        // FK
        public Guid MatchId { get; set; }
        public Guid ItemId { get; set; }

        // Navigation Property
        public Match Match { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}