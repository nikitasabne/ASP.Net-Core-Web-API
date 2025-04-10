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
        private readonly CloudinaryService cloudinaryService;

        public IMapper Mapper { get; }

        public RegionsController(IRegionRepository regionRepository, CloudinaryService cloudinaryService, IMapper mapper)    //calling DbContext so that we can invoke Database instances(i.e tables)
        {
            this.regionRepository = regionRepository;
            this.cloudinaryService = cloudinaryService;
            Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllRegion()
        {
            var regions = await regionRepository.GetAllRegion();

            //Destination:RegionDto, Source: regions
            var regionDto = Mapper.Map<List<RegionDto>>(regions);
            return Ok(regionDto);
        }

        // Provide the image's public ID from Cloudinary, and this function will return a signed, secure URL.
        // This URL gives authorized access to the image and can only be used by you.
        // You can also call this method from another function to get the secured/private image URL.
        [HttpGet("get-private-image")]
        public IActionResult GetPrivateImage([FromQuery] string publicId)
        {
            var url = cloudinaryService.GetPrivateImageUrl(publicId);
            return Ok(new { imageUrl = url });
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader,Writer")]
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
        public async Task<IActionResult> Create([FromForm] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = Mapper.Map<Region>(addRegionRequestDto);
            if(addRegionRequestDto.RegionImageUrl != null)
            {
                string imageUrl = await cloudinaryService.UploadImageAsync(addRegionRequestDto.RegionImageUrl);
                regionDomainModel.RegionImageUrl = imageUrl;
            }
            await regionRepository.CreateRegionAsync(regionDomainModel);
            return Ok("Region created successfully");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = Mapper.Map<Region>(updateRegionRequestDto);
            if(updateRegionRequestDto.RegionImageUrl != null)
            {
                string imageUrl = await cloudinaryService.UploadImageAsync(updateRegionRequestDto.RegionImageUrl);
                regionDomainModel.RegionImageUrl = imageUrl;
            }
            await regionRepository.UpdateRegionAsync(id, regionDomainModel);
            //return Ok("Region updated successfully");
            return Ok();

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await regionRepository.DeleteRegionAsync(id);
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Create a unique file name
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Build a public URL to access the image
            var imageUrl = $"{Request.Scheme}://{Request.Host}/Images/{fileName}";

            return Ok(new { imageUrl });
        }

    }
}
