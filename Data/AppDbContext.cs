using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AmiSocialWebApi.Models;

namespace AmiSocialWebApi.Data
{
    public class AppDbContext : IdentityDbContext<AmiUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // modelBuilder.Entity<Competition>(c =>
        //     // {
        //     //     c.Property(c => c.PlayInAmount).HasColumnType("money");
        //     // });
        // }
    }
}