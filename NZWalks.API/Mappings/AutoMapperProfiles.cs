using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles: Profile
    { 
        
        public AutoMapperProfiles()
        {
            //mapping between DTO and DM
            CreateMap<Region, RegionDTO>().ReverseMap(); 
            CreateMap<AddRegionRequestDTO, Region>().ReverseMap();
        }
    }
}
