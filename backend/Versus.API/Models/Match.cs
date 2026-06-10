namespace Versus.API.Models
{
    public class Match
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset PlayedAt { get; set; } = DateTimeOffset.UtcNow;

        // FK
        public Guid TierListId { get; set; }
        public Guid SessionId { get; set; }

        public Guid WinnerId { get; set; }
        public Guid LoserId { get; set; }

        public bool IsPlayAgain { get; set; } = false;

        // Navigation Property
        public TierList TierList { get; set; } = null!;
        public Session Session { get; set; } = null!;
        public Item Winner { get; set; } = null!;
        public Item Loser { get; set; } = null!;
        public ICollection<EloHistory> EloHistories { get; set; } = [];
    }
}