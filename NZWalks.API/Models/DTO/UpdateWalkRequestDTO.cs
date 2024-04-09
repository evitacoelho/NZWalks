using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDTO
    {

        [Required]
        [MaxLength(100, ErrorMessage = "Name can be a maximum of 70 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Description can be a maximum of 500 characters")]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
