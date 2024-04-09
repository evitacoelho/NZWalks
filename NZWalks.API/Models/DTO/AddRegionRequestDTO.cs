using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        //Guid is generated and is not part of the client request to create a DTO
        //use data annotation for validation
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be exactly 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be exactly 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name can be a maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
