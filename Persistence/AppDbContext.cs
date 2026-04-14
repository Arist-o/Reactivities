using Application.Interfaces;
using Domain;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<User>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<Domain.Report> Reports { get; set; }

        void IAppDbContext.Remove<TEntity>(TEntity entity)
        {
            base.Remove(entity);
        }
        public async Task BulkCreateAreasWithCentersAsync(
       List<Domain.Area> areas,
       List<Domain.City> cities,
       CancellationToken ct)
        {
            await this.BulkInsertAsync(areas, cancellationToken: ct);

            for (int i = 0; i < areas.Count; i++)
            {
                cities[i].AreaId = areas[i].Id;
            }

            await this.BulkInsertAsync(cities, cancellationToken: ct);

            for (int i = 0; i < areas.Count; i++)
            {
                areas[i].AreaCenterId = cities[i].Id;
            }

            await this.BulkUpdateAsync(areas, cancellationToken: ct);
        }
        public IExecutionStrategy CreateExecutionStrategy() => Database.CreateExecutionStrategy();

        public async Task BeginTransactionAsync(CancellationToken ct)
            => await Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct)
        {
            if (Database.CurrentTransaction != null)
                await Database.CurrentTransaction.CommitAsync(ct);
        }

        public async Task RollbackTransactionAsync(CancellationToken ct)
        {
            if (Database.CurrentTransaction != null)
                await Database.CurrentTransaction.RollbackAsync(ct);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Area>(entity =>
            {
                entity.Property(e => e.description).IsRequired().HasMaxLength(150);

                entity.HasOne(e => e.area_center)
                    .WithOne()
                    .HasForeignKey<Area>(e => e.AreaCenterId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });

            builder.Entity<City>(entity =>
            {
                entity.Property(e => e.description).IsRequired().HasMaxLength(150);

                entity.HasOne(e => e.area)
                    .WithMany(a => a.Cities)
                    .HasForeignKey(e => e.AreaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Street>(entity =>
            {
                entity.Property(e => e.description).IsRequired().HasMaxLength(150);

                entity.HasOne(e => e.city)
                    .WithMany(c => c.streets)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Cascade);


            });

            builder.Entity<WareHouse>(entity =>
            {
                entity.Property(e => e.description).IsRequired().HasMaxLength(150);
                entity.HasOne(e => e.city)
                    .WithMany(c => c.wareHouses)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Report>(entity =>
            {
                entity.Property(e => e.email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.phone).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.Area)
                    .WithMany()
                    .HasForeignKey(e => e.AreaId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.City)
                    .WithMany()
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(e => e.Street)
                    .WithMany()
                    .HasForeignKey(e => e.StreetId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.WareHouse)
                    .WithMany()
                    .HasForeignKey(e => e.WareHouseId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

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

            builder.Entity<UserFollowing>(x =>
            {
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

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }
        }
    }
        //public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
        //{
        //    public required DbSet<Activity> Activities { get; set; }

        //    public required DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        //    public required DbSet<Photo> Photos { get; set; }

        //    public required DbSet<Comment> Comments { get; set; }

        //    public required DbSet<UserFollowing> UserFollowings { get; set; }

        //    public required DbSet<Area> Areas { get; set; }

        //    public required DbSet<City> Cities { get; set; }

        //    public required DbSet<Street> Streets { get; set; }

        //    public required DbSet<WareHouse> WareHouses { get; set; }

        //    public required DbSet<Report> Reports { get; set; }



        //    protected override void OnModelCreating(ModelBuilder builder)
        //    {
        //        base.OnModelCreating(builder);

        //        builder.Entity<Area>(entity =>
        //        {
        //            entity.Property(e => e.description).IsRequired().HasMaxLength(150);

        //            entity.HasOne(e => e.area_center)
        //                .WithOne()
        //                .HasForeignKey<Area>(e => e.AreaCenterId)
        //                .IsRequired(false)
        //                .OnDelete(DeleteBehavior.ClientNoAction);
        //        });

        //        builder.Entity<City>(entity =>
        //        {
        //            entity.Property(e => e.description).IsRequired().HasMaxLength(150);

        //            entity.HasOne(e => e.area)
        //                .WithMany(a => a.Cities)
        //                .HasForeignKey(e => e.AreaId)
        //                .OnDelete(DeleteBehavior.Restrict);
        //        });

        //        builder.Entity<Street>(entity =>
        //        {
        //            entity.Property(e => e.description).IsRequired().HasMaxLength(150);

        //            entity.HasOne(e => e.city)
        //                .WithMany(c => c.streets)
        //                .HasForeignKey(e => e.CityId)
        //                .OnDelete(DeleteBehavior.Cascade);


        //        });

        //        builder.Entity<WareHouse>(entity =>
        //        {
        //            entity.Property(e => e.description).IsRequired().HasMaxLength(150);
        //            entity.HasOne(e => e.city)
        //                .WithMany(c => c.wareHouses)
        //                .HasForeignKey(e => e.CityId)
        //                .OnDelete(DeleteBehavior.Cascade);
        //        });

        //        builder.Entity<Report>(entity =>
        //        {
        //            entity.Property(e => e.email).IsRequired().HasMaxLength(150);
        //            entity.Property(e => e.phone).IsRequired().HasMaxLength(20);

        //            entity.HasOne(e => e.Area)
        //                .WithMany()
        //                .HasForeignKey(e => e.AreaId)
        //                .OnDelete(DeleteBehavior.NoAction);

        //            entity.HasOne(e => e.City)
        //                .WithMany()
        //                .HasForeignKey(e => e.CityId)
        //                .OnDelete(DeleteBehavior.NoAction);


        //            entity.HasOne(e => e.Street)
        //                .WithMany()
        //                .HasForeignKey(e => e.StreetId)
        //                .OnDelete(DeleteBehavior.NoAction);  

        //            entity.HasOne(e => e.WareHouse)
        //                .WithMany()
        //                .HasForeignKey(e => e.WareHouseId)
        //                .OnDelete(DeleteBehavior.NoAction);
        //        });

        //        builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.ActivityId, a.UserId }));

        //        builder.Entity<ActivityAttendee>()
        //            .HasOne(x => x.User)
        //            .WithMany(x => x.Activities)
        //            .HasForeignKey(x => x.UserId);

        //        builder.Entity<ActivityAttendee>()
        //            .ToTable("ActivityAttendees")
        //            .HasOne(x => x.Activity)
        //            .WithMany(x => x.Attendees)
        //            .HasForeignKey(x => x.ActivityId);

        //        builder.Entity<UserFollowing>(x => { 
        //            x.HasKey(k => new { k.ObserverId, k.TargetId });

        //            x.Property(f => f.ObserverId).HasMaxLength(450); 
        //            x.Property(f => f.TargetId).HasMaxLength(450);

        //            x.HasOne(o => o.Observer)
        //                .WithMany(f => f.Followings)
        //                .HasForeignKey(o => o.ObserverId)
        //                .OnDelete(DeleteBehavior.Restrict);

        //            x.HasOne(o => o.Target)
        //                .WithMany(f => f.Followers)
        //                .HasForeignKey(o => o.TargetId)
        //                .OnDelete(DeleteBehavior.Cascade);
        //        });

        //        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
        //            v => v.ToUniversalTime(),
        //            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        //        foreach (var entityType in builder.Model.GetEntityTypes()) {
        //            foreach (var property in entityType.GetProperties()) {
        //                if (property.ClrType == typeof(DateTime)) {
        //                    property.SetValueConverter(dateTimeConverter);
        //                }
        //            }
        //        }
        //    }


        //}

    
}
