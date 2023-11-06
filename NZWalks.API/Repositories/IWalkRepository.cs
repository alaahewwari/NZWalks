using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        public Task<Walk> CreateAsync(Walk walk);
        public Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 1000);

        public Task<Walk?> GetWalkByIdAsync(Guid id);
        public Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        public Task<Walk?> DeleteWalkAsync(Guid id);

    }
}
