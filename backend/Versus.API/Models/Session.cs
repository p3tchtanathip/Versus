namespace Versus.API.Models
{
    public class Session
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastSeenAt { get; set; }

        // Navigation Property
        public ICollection<TierList> TierLists { get; set; } = [];
        public ICollection<Match> Matches { get; set; } = [];
    }
}