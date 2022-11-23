namespace Restaurant.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage { get; }

        public int PreviousPageNumber { get; }

        public bool HasNextPage { get; }

        public int NextPageNumber { get; }

        public int PagesCount { get; }

        public int ItemCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
