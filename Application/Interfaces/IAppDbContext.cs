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
        DbSet<Activity> Activities { get; }

        DbSet<ActivityAttendee> ActivityAttendees { get; }

        DbSet<Photo> Photos { get; }

        DbSet<Comment> Comments { get;  }

        DbSet<UserFollowing> UserFollowings { get; }

        DbSet<Area> Areas { get; }

        DbSet<City> Cities { get;}

        DbSet<Street> Streets { get; }

        DbSet<WareHouse> WareHouses { get; }
        DbSet<User> Users { get; }

        DbSet<Domain.Report> Reports { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task BeginTransactionAsync(CancellationToken ct);
        Task CommitTransactionAsync(CancellationToken ct);
        Task RollbackTransactionAsync(CancellationToken ct);
        IExecutionStrategy CreateExecutionStrategy();

        void Remove<TEntity>(TEntity entity) where TEntity : class;

    }
}
