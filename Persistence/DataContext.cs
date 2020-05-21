using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserFollowing> Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserActivity>(x => x.HasKey(ua => new { ua.AppUserId, ua.ActivityId }));

            modelBuilder.Entity<UserActivity>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(u => u.AppUserId);

            modelBuilder.Entity<UserActivity>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(u => u.ActivityId);

            modelBuilder.Entity<UserFollowing>(b =>
            {
                b.HasKey(k => new { k.ObserverId, k.TargetId });

                b.HasOne(o => o.Observer)
                    .WithMany(o => o.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(o => o.Target)
                    .WithMany(o => o.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
