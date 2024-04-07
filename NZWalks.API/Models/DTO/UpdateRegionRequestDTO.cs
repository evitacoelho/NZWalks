namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDTO
    {
        //only allow these values to be updated
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
