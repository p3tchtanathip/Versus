namespace Versus.API.DTOs.Responses
{
    public class TierGroupResponse
    {
        public string TierLabel { get; set; } = string.Empty;
        public List<TierResultItemResponse> Items { get; set; } = [];
    }
}
