using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    //repostory is responsible to use the dbcontext to talk to the DB
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        //inject the dbcontext in the contructor
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Get a list of all regions using db context
        public async Task<List<Region>> GetAllAsync()
        {
           return await _dbContext.Regions.ToListAsync();  
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x =>x.Id == id);
            if (existingRegion == null) 
            {
                return null;
            }
            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await _dbContext.SaveChangesAsync();
            return existingRegion;
           
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            //existing region
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            _dbContext.Regions.Remove(existingRegion);
            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
