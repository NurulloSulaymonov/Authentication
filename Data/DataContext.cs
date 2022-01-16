using Authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{

    
    public class DataContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        DbSet<Teacher> Teachers {get;set;}
    }
}
