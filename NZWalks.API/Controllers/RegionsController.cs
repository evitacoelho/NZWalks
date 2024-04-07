using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private NZWalksDbContext _dbContext;

        //create a controller to inject DbContext - used by the controller to talk to the DB
        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Action that is executed to get all regions
        //GET: //https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //DbContext - bridge between controller and DB
            //Get data from the DB - using domain models and Db context
            var regionsDomain = _dbContext.Regions.ToList();

            //Map Domain models to DTOs
            //extract indivdiual region from region domain model and map it into a region dto list
            var regionsDTO = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            //Return DTOs to the requesting view
            return Ok(regionsDTO);
        }

        //Action method to get a single region by Id
        //GET: //https://localhost:portnumber/api/regions/{id}
        //both params must use the same name --> id
        //type safety --> make id Guid in the Route
        //FromRoute indicates the id is taken from the URL route
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = _dbContext.Regions.Find(id); //find can only be used with PK
            //Get the region domanin model
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            else
            {
                //map region DM to region DTO
                var regionDTO = new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                };

                //return DTO to client
                return Ok(regionDTO);
            }
        }
        //Action method to create a region
        //PUT: //https://localhost:portnumber/api/regions
        //The request to create a region is in part of the Post request body
        //comes from the addrequest dto 
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map DTO to domain model
            var regionDomainModel = new Region
            {
                Name = addRegionRequestDTO.Name,
                Code = addRegionRequestDTO.Code,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            //Use the DM to create a region using db context
            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

            //Map domain model back to DTO
            var regionDTO = new RegionDTO
            {
                //Id is created by EF Core
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //return a response to the client
            //this is a 201 response and is created using CreatedAtAction()
            //Param: Action method - use GetById() as it gets a single resource 
            //Param: New id of the domain model
            //Param: DTO id to send back to client
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDTO);
        }

        //Action method to update a region
        //PUT: //https://localhost:portnumber/api/regions {id}
        [HttpPost]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //extract the record from the domain model from the id passed
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

            //if region does not exist
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //if region exisits - update it
            //Map domain model to DTO
            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            _dbContext.SaveChanges();

            //Convert domain model to DTO
            var regionDTO = new RegionDTO
            {
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            //return dto to client 
            return Ok(regionDTO);
        }

        //Delete a region by id
        //DELETE: //https://localhost:portnumber/api/regions {id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            //Find region with matching Id
            var regionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

            //If region not found
            if (regionDomainModel == null)
                return NotFound();
           
            //Delete region with matching Id and update DB
            _dbContext.Regions.Remove(regionDomainModel);
            _dbContext.SaveChanges();

            return Ok();

        }
    }
}
