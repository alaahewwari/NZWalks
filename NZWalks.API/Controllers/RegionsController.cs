using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
namespace NZWalks.API.Controllers
{
    //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> logger;
        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository ,
            IMapper mapper, ILogger<RegionsController> logger) {
            _context= dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            this.logger = logger;
        }

        //get all regions
        // GET: http://localhost:5000/api/regions
        //[Authorize(Roles = "Reader")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
                //get data from database - domain models 
                var regionDomainModel = await _regionRepository.GetAllAsync();
                //Map domain models to DTOs
                var regionDTO = _mapper.Map<List<RegionDTO>>(regionDomainModel);
                //return DTOs
                return Ok(regionDTO);
           
            
        }
        //get region by id
        //GET: http://localhost:5000/api/regions/{1}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //get region domain model from database
            var regionDomainModel = await _regionRepository.GetByIdAsync(id);
            //if region is null
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDTO);


        }

        //post to create new region
        //POST: http://localhost:5000/api/regions
        [HttpPost]
        [ValidateModel]
       // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            
            //Map or convert DTO to domain model
            var regionDomainModel= _mapper.Map<Region>(addRegionRequestDTO);
            //use domain model to create new region in database (it gives id to domain model)
            await _regionRepository.CreateAsync(regionDomainModel);

            //map domain model to DTO 
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

            //return 201 status code by CreatedAtAction
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDTO.Id }, regionDTO);
        }

        //put to update region by id
        //PUT: http://localhost:5000/api/regions/{id}
         [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Map DTO to domain model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);
            //get region domain model from database by id after map or convert DTO to domain model
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
            //check if region is null
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
            //return 200 status code by Ok
            return Ok(regionDTO);
        }


        //delete region by id
        //http://localhost:5000/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
      //  [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //remove region domain model from database
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            //return 204 status code by NoContent
            //return Ok();
            //return deleted region domain model by create a regionDTO
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDTO);
            

        }
    }
}
