using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<ActivityAttendee> ActivityAttendees { get; set; }
         DbSet<Photo> Photos { get; set; }
        DbSet<Comment> Comments { get; set; }
         DbSet<UserFollowing> UserFollowings { get; set; }
        DbSet<Area> Areas { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Street> Streets { get; set; }
        DbSet<WareHouse> WareHouses { get; set; }
        DbSet<Domain.Report> Reports { get; set; }
        DbSet<Domain.User> Users { get; }

        Task BulkCreateAreasWithCentersAsync(List<Domain.Area> areas, List<Domain.City> cities, CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task BeginTransactionAsync(CancellationToken ct);
        Task CommitTransactionAsync(CancellationToken ct);
        Task RollbackTransactionAsync(CancellationToken ct);
        IExecutionStrategy CreateExecutionStrategy();

        void Remove<TEntity>(TEntity entity) where TEntity : class;

    }
}
