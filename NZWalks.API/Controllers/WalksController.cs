using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Threading.Tasks;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            WalkRepository = walkRepository;
            this.mapper = mapper;
        }

        public IWalkRepository WalkRepository { get; }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortByName, [FromQuery] bool isAsc, [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 100)
        {
            var walks = await WalkRepository.GetAllWalks(filterOn, filterQuery, sortByName, isAsc = true, pageNumber, pageSize);
            var walkDto = mapper.Map<List<WalkDto>>(walks);
            return Ok(walkDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkById(Guid id)
        {
            var walk = await WalkRepository.GetWalkById(id);
            var getWalk = mapper.Map<WalkDto>(walk);
            return Ok(getWalk);

        }

        [HttpPost]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walk = mapper.Map<Walk>(addWalkRequestDto);
            await WalkRepository.CreateWalk(walk);
            return Ok("Walk created successfully");
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateWalk(Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walk = mapper.Map<Walk>(updateWalkRequestDto);
            await WalkRepository.UpdateWalk(id, walk);
            return Ok("Walk Updated successfully");
        }
    }
}
