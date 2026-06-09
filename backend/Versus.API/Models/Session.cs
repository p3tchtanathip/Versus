namespace Versus.API.Models
{
    public class Session
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastSeenAt { get; set; }

        // Navigation Property
        public ICollection<TierList> TierLists { get; set; } = [];
        public ICollection<Match> Matches { get; set; } = [];
    }
}