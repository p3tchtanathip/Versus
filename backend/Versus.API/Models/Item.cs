using System.ComponentModel.DataAnnotations;

namespace Versus.API.Models
{
    public class Item
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public float EloRating { get; set; } = 1000;
        public int MatchCount { get; set; } = 0;
        public string? ExternalId { get; set; }
        public string? ExternalSource { get; set; }

        // FK
        public Guid TierListId { get; set; }

        // Concurrency
        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        // Navigation Property
        public TierList TierList { get; set; } = null!;
        public ICollection<Match> WinnerMatches { get; set; } = [];
        public ICollection<Match> LoserMatches { get; set; } = [];
        public ICollection<EloHistory> EloHistories { get; set; } = [];
    }
}