using Application.Activities.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = [];
        public PaginationMetadata Metadata { get; set; } = new();
    }
}
