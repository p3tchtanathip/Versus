using System.ComponentModel.DataAnnotations;
using Versus.API.Models.Enums;

namespace Versus.API.DTOs.Requests
{
    public class SearchRequest
    {
        [Required(ErrorMessage = "Query is required")]
        [MinLength(1)]
        [MaxLength(100)]
        public string Query { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public TierListCategory? Category { get; set; }

        public SportsSearchType? Type { get; set; }
    }
}