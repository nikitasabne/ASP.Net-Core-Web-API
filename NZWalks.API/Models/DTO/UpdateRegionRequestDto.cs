using Microsoft.Extensions.FileProviders;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IFormFile? RegionImageUrl { get; set; } 
    }
}
