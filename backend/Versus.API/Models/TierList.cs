namespace Versus.API.Models
{
    public class TierList
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        // FK
        public Guid CreatorSessionId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public Session CreatorSession { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = [];
        public ICollection<Match> Matches { get; set; } = [];
    }
}