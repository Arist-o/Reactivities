using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel.Resolution;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace Persistence
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public required DbSet<Activity> Activities { get; set; }

        public required DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        public required DbSet<Photo> Photos { get; set; }

        public required DbSet<Comment> Comments { get; set; }

        public required DbSet<UserFollowing> UserFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.ActivityId, a.UserId }));

            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.User)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.UserId);

            builder.Entity<ActivityAttendee>()
                .ToTable("ActivityAttendees")
                .HasOne(x => x.Activity)
                .WithMany(x => x.Attendees)
                .HasForeignKey(x => x.ActivityId);

            builder.Entity<UserFollowing>(x => { 
                x.HasKey(k => new { k.ObserverId, k.TargetId });

                x.Property(f => f.ObserverId).HasMaxLength(450); 
                x.Property(f => f.TargetId).HasMaxLength(450);

                x.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                x.HasOne(o => o.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in builder.Model.GetEntityTypes()) {
                foreach (var property in entityType.GetProperties()) {
                    if (property.ClrType == typeof(DateTime)) {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }
        }


    }
}
