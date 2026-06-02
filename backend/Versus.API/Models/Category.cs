namespace Versus.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation Property
        public ICollection<TierList> TierLists { get; set; } = [];
    }
}