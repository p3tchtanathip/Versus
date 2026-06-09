using System.ComponentModel.DataAnnotations;

namespace Versus.API.DTOs.Requests
{
    public class CreateTierListItemRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        public string? ExternalId { get; set; }
        public string? ExternalSource { get; set; }
    }
}
