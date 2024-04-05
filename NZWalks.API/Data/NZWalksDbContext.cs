using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext 
    {
        // this is used by the controllers to talk to the DB
        //pass the db context options to the base class
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base (dbContextOptions)
        {
            
        }
        //DbSet represents a collection of entities in the DB i.e. domain models (Walk, Region, Difficulty)
        //Naming Convention: Prop names are plural of the domain name - creates tables within the DB based on the domain model
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

    }
}
