namespace Versus.API.DTOs.Responses
{
    public class SearchResponse
    {
        public string? ExternalId { get; set; }
        public string? ExternalSource { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Metadata { get; set; }
    }
}