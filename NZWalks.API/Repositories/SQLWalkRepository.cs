using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq;

namespace NZWalks.API.Repositories

{
    public class SQLWalkRepository:IWalkRepository
    {
        private readonly NZWalksDbContext _context;
        public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            _context = nZWalksDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var walk = await _context.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }
            _context.Walks.Remove(walk);
            await _context.SaveChangesAsync();
            return walk;
        }
        public async Task<List<Walk>> GetAllAsync(string?
            filterOn= null, string? filterQuery = null, 
            string? sortBy =  null,bool? isAscending = true,
            int pageNumber=1 ,int pageSize=1000)
        {
            var walks = _context.Walks
               .Include("Difficulty")
               .Include("Region")
               .AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                   if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));

                }
            }
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    if (isAscending == true)
                    {
                        walks = walks.OrderBy(x => x.Name);
                    }
                    else
                    {
                        walks = walks.OrderByDescending(x => x.Name);
                    }
                }
                else if (sortBy.Equals("length",StringComparison.OrdinalIgnoreCase))
                {
                    if(isAscending == true)
                    {
                        walks = walks.OrderBy(x => x.LengthInKm);
                    }
                    else
                    {
                        walks = walks.OrderByDescending(x => x.LengthInKm);
                    }
                }
            }
            //pagination
            var skipResults=pageSize * (pageNumber - 1);
            walks = walks.Skip(skipResults).Take(pageSize);
            return (await walks.ToListAsync());
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            var existWalk=await _context.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=> x.Id == id);
            return existWalk;
        }
        public async Task<Walk?> UpdateWalkAsync(Guid id,Walk walk)
        {
            var existWalk = await _context.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=> x.Id == id);
            if(existWalk == null)
            {
                return null;
            }
            existWalk.Name = walk.Name;
            existWalk.Description = walk.Description;
            existWalk.WalkImageUrl = walk.WalkImageUrl;
            existWalk.LengthInKm = walk.LengthInKm;
            existWalk.DifficultyId = walk.DifficultyId;
            existWalk.RegionId = walk.RegionId;
            await _context.SaveChangesAsync();
            return existWalk;
        }
    }
}
