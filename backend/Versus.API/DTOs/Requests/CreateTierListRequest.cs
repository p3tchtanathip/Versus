using System.ComponentModel.DataAnnotations;

namespace Versus.API.DTOs.Requests
{
    public class CreateTierListRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MinLength(2)]
        public List<CreateTierListItemRequest> Items { get; set; } = [];
    }
}