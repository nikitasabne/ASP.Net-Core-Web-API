using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Threading.Tasks;

namespace NZWalks.API.Controllers
{
    //url='http://localhost:5075/api/regions'
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;

        public IMapper Mapper { get; }

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)    //calling DbContext so that we can invoke Database instances(i.e tables)
        {
            this.regionRepository = regionRepository;
            Mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegion()
        {
            var regions = await regionRepository.GetAllRegion();

            //Destination:RegionDto, Source: regions
            var regionDto = Mapper.Map<List<RegionDto>>(regions);
            return Ok(regionDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute]Guid id)
        {
            //Get data from Database - Domain model
            var oneRegion = await regionRepository.GetRegionByIdAsync(id);

            if(oneRegion == null)
            {
                return NotFound();
            }

            var regionDto = Mapper.Map<RegionDto>(oneRegion);
            
            //Return DTO
            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = Mapper.Map<Region>(addRegionRequestDto);
            await regionRepository.CreateRegionAsync(regionDomainModel);
            return Ok("Region created successfully");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = Mapper.Map<Region>(updateRegionRequestDto);
            await regionRepository.UpdateRegionAsync(id, regionDomainModel);  
            return Ok("Region updated successfully");
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await regionRepository.DeleteRegionAsync(id);
            return Ok("Region deleted successfully");
        }
    }
}
