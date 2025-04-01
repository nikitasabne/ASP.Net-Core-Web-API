using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repository
{
    public class SQLRegionRepository : IRegionRepository
    {
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public NZWalksDbContext DbContext { get; }

        public async Task<Region?> CreateRegionAsync(Region region)
        {
            await DbContext.regions.AddAsync(region);
            await DbContext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllRegion()
        {
            return await DbContext.regions.ToListAsync();
        }

        public async Task<Region> GetRegionByIdAsync(Guid id)
        {
            return await DbContext.regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
        {
            var oneRegion = await DbContext.regions.FirstOrDefaultAsync(x => x.Id == id);

            if(oneRegion == null)
            {
                return null;
            }
            oneRegion.Code = region.Code;
            oneRegion.Name = region.Name;
            oneRegion.RegionImageUrl = region.RegionImageUrl;

            await DbContext.SaveChangesAsync();
            return oneRegion;
        }

        public async Task<Region?> DeleteRegionAsync(Guid id)
        {
            var deleteRegion = await DbContext.regions.FirstOrDefaultAsync(x => x.Id == id);
            DbContext.regions.Remove(deleteRegion);
            await DbContext.SaveChangesAsync();
            return deleteRegion;
        }
    }
}
