namespace Versus.API.DTOs.Requests
{
    public class TierListQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? CategoryId { get; set; }
        public string? Search { get; set; }
    }
}