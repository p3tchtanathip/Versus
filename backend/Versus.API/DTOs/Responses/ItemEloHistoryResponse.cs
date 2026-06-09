namespace Versus.API.DTOs.Responses
{
    public class ItemEloHistoryResponse
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public float InitialRating { get; set; } = 1000;
        public List<EloHistoryGraphPointResponse> Points { get; set; } = [];
    }
}
