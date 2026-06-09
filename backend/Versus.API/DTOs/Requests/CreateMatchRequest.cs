using System.ComponentModel.DataAnnotations;

namespace Versus.API.DTOs.Requests
{
    public class CreateMatchRequest
    {
        [Required]
        public Guid TierListId { get; set; }

        [Required]
        public Guid WinnerId { get; set; }

        [Required]
        public Guid LoserId { get; set; }
    }
}
