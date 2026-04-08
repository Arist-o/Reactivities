using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAppDbContext _context;

        public TransactionBehavior(IAppDbContext context) => _context = context;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.GetType().Name.EndsWith("Query"))
            {
                return await next();
            }

            var executionStrategy = _context.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                await _context.BeginTransactionAsync(cancellationToken);

                try
                {
                    var response = await next();
                    await _context.CommitTransactionAsync(cancellationToken);
                    return response;
                }
                catch (Exception)
                {
                    await _context.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            });
        }
    }
}