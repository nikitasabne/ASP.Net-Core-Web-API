using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllWalks(string? filterOn = null, string? filterQuery = null, 
            string? sortByName = null, bool isAsc = true, int pageNumber = 1, int pageSize = 100);
        Task<Walk> CreateWalk(Walk walk);
        Task<Walk> GetWalkById(Guid id);
        Task<Walk> UpdateWalk(Guid id, Walk walk);
    }
}
