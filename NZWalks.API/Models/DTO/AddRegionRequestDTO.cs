namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        //Guid is generated and is not part of the client request to create a DTO
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
