using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        [NotMapped]
        [Required]
        public IFormFile File { get; set; }
        public string? FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string? FilePath { get; set; }
    }
}
