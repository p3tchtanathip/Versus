using System.ComponentModel.DataAnnotations;

namespace Versus.API.DTOs.Requests
{
    public class CreateTierListRequest
    {
        [Required]
        public string Name { get; set; }
    }
}