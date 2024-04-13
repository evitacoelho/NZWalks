using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        //this additional DbContext is used to seed identity roles
        public NZWalksAuthDbContext(DbContextOptions <NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //generate Guis= for reader and write roles
            var readerRoleId = "5daade09-5d4b-4dec-839e-b51e572eb071";
            var writerRoleId = "4f1e6495-4931-4f6c-9369-776c5082e88b";

            //seed different roles based on Identity Framework
            var roles = new List<IdentityRole>
            {
                //these properties are predefined in Identity Role class
               new IdentityRole
               {
                   Id = readerRoleId,
                   Name = "Reader",
                   ConcurrencyStamp = readerRoleId,
                   NormalizedName = "Reader".ToUpper()
               },
               new IdentityRole
               {
                   Id = writerRoleId,
                   Name = "Writer",
                   ConcurrencyStamp = writerRoleId,
                   NormalizedName = "Writer".ToUpper()
               }

            };
            //seed data
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
