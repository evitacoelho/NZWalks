using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        //create a controller to inject DbContext - used by the controller to talk to the DB
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        //Action that is executed to get all regions
        //GET: //https://localhost:portnumber/api/regions
        //All Async methods return a <Task> type - wraps the result in Task
        //All calls made by an async method have to implement await
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //DbContext - bridge between controller and DB
            //Get data from the DB - using domain models and Db context
            // var regionsDomain = await _dbContext.Regions.ToListAsync();

            //replace the dbcontext with repository interface
            var regionsDomain = await _regionRepository.GetAllAsync();

            //Map Domain models to DTOs
            //extract indivdiual region from region domain model and map it into a region dto list
            //var regionsDTO = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDTO.Add(new RegionDTO()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            //map Domain model to DTO
            //Params: Destination type, Source
            var regionsDTO = _mapper.Map<List<RegionDTO>>(regionsDomain);
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
        public async Task<IActionResult>  GetById([FromRoute] Guid id)
        {
            
            //Get the region domanin model
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            else
            {
               
                var regionDTO = _mapper.Map<RegionDTO>(regionDomain);
                //return DTO to client
                return Ok(regionDTO);
            }
        }
        //Action method to create a region
        //PUT: //https://localhost:portnumber/api/regions
        //The request to create a region is in part of the Post request body
        //comes from the addrequest dto 
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
           
               var regionDomainModel = _mapper.Map<Region>(addRegionRequestDTO);
                //Use the DM to create a region using db context
                await _regionRepository.CreateAsync(regionDomainModel);


                //Map domain model back to DTO
                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
                //return a response to the client
                //this is a 201 response and is created using CreatedAtAction()
                //Param: Action method - use GetById() as it gets a single resource 
                //Param: New id of the domain model
                //Param: DTO id to send back to client
                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDTO);
           
           
        }

        //Action method to update a region
        //PUT: //https://localhost:portnumber/api/regions {id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
                //extract the record from the domain model from the id passed - done in repo
                //Map DTO to Domain Model
                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);

                await _regionRepository.UpdateAsync(id, regionDomainModel);

                //if region does not exist
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Repo does the update activity

                //Convert domain model to DTO
                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
                //return dto to client 
                return Ok(regionDTO);
            
        }

        //Delete a region by id
        //DELETE: //https://localhost:portnumber/api/regions {id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Find region with matching Id
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            //If region not found
            if (regionDomainModel == null)
                return NotFound();

            //Delete region with matching Id and update DB - done in repo
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDTO);

        }
    }
}
