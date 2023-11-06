using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers

{   //https://localhost:1234/api/walks
    [Route("api/[controller]")]
    [ApiController]

    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpPost] //https://localhost:1234/api/walks
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
           
            //map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);
            
            //call repo to add to db
            await walkRepository.CreateAsync(walkDomainModel);

            //map domain model back to dto
            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }


        [HttpGet] //https://localhost:1234/api/walks/fillterOn=Name&filterQuery=Abel&sortOn=Name&sortDirection=asc
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                                                [FromQuery] int pageNumber = 1, [FromQuery]int pageSize = 1000)
        {
            
                var walkDomainModels = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
                var walkDTOs = mapper.Map<List<WalkDTO>>(walkDomainModels);
                return Ok(walkDTOs);
        }

        //GET https://localhost:1234/api/walks/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetWalkById([FromRoute]Guid id)
        {
            var walkDomainModel = await walkRepository.GetWalkByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }   
            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, UpdateWalkRequestDTO updateWalkRequestDTO)
        {
           
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDTO);
            walkDomainModel=await walkRepository.UpdateWalkAsync(id, walkDomainModel);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<UpdateWalkRequestDTO>(walkDomainModel);
            return Ok(walkDTO);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteWalkAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }
    }
}
