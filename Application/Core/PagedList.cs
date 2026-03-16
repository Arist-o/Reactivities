using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core
{
    public class PagedList<T,TCursor>
    {
        public List<T> Items { get; set; } = [];

        public TCursor? NextCutsor { get; set; }
    }
}
