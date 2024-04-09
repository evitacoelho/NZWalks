using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.CompilerServices;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }



        //Create Walk
        //POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
           
                //Map the DTO to DM
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);

                await _walkRepository.CreateAsync(walkDomainModel);

                //Map DM to DTO
                var walkDTO = _mapper.Map<Walk>(walkDomainModel);

                return Ok(walkDTO);
            

        }

        //Get all walks
        //GET: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await _walkRepository.GetAllAsync();
            //Map DM to DTO
            var walksDTO = _mapper.Map<List<Walk>>(walksDomainModel);

            return Ok(walksDTO);

        }
        //Get walk by id
        //GET: /api/walks{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
                return NotFound();

            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }

        //Update walk by id
        //PUT: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);

                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                    return NotFound();
                var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
                return Ok(walkDTO);
            

        }

        //Delete walk by Id
        //DELETE: /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            if(walkDomainModel == null)
                return NotFound();
            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);

        }

    }
}
