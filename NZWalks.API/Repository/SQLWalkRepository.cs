using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repository
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateWalk(Walk walk)
        {
            await dbContext.walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllWalks(string? filterOn = null, string? filterQuery = null, 
            string? sortByName = null, bool isAsc = true, int pageNumber = 1, int pageSize = 100)
        {

            //Filter
            var walks = dbContext.walks.Include("Difficulty").Include("Region").AsQueryable();
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sort
            if(string.IsNullOrWhiteSpace(sortByName) == false)
            {
                if(sortByName.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAsc ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            //return await walks.ToListAsync();
        }

        public Task<Walk> GetWalkById(Guid id)
        {
            var walk = dbContext.walks.FirstOrDefaultAsync(x => x.Id == id);            
            return walk;
        }

        public async Task<Walk> UpdateWalk(Guid id, Walk walk)
        {
            var getWalk = await dbContext.walks.FirstOrDefaultAsync(x => x.Id == id);

            getWalk.Name = walk.Name;
            getWalk.Description = walk.Description;
            getWalk.LengthInKm = walk.LengthInKm;
            getWalk.DifficultyId = walk.DifficultyId;
            getWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();
            return getWalk;
        }
    }
}
