﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDTO
    {
        //only allow these values to be updated
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be exactly 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be exactly 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name can be a maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
