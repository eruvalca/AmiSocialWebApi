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

        public DbSet<Post> Posts { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // modelBuilder.Entity<Post>()
        //     //     .HasOne<AmiUser>()
        //     //     .WithMany()
        //     //     .HasForeignKey(p => p.AmiUserId);
        //     // modelBuilder.Entity<Competition>(c =>
        //     // {
        //     //     c.Property(c => c.PlayInAmount).HasColumnType("money");
        //     // });
        // }
    }
}