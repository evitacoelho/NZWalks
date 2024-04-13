using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
            
        }

       
        public async Task<List<Walk>> GetAllAsync(string? filterOn=null, string? filterQuery=null, string? 
            sortBy=null, bool isAscending=true, int pageNumber = 1, int pageSize = 100)
        {
            //get walks based on filtering and querying
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering based on query -Name
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false )
            { 
                //filter based on name ignoring case (query parameter is a subset of name)
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase) )
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
            }

            //sorting based on name or length
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                //sorting based on name
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                else
                //sorting based on length
                if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }

            //pagination - based on skip and take 
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();   
            //include - includes navigation props from related models as needed
            //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk =await _dbContext.Walks.FirstOrDefaultAsync (x => x.Id == id);
            if(existingWalk == null) 
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;

            await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            _dbContext.Walks.Remove(existingWalk);
            await _dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
