namespace Restaurant.Web.ViewModels
{
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
