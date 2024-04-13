using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class ImageUploadRequestDTO
    {
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
       
        [Required]
        public IFormFile File { get; set; }
       
    }
}
