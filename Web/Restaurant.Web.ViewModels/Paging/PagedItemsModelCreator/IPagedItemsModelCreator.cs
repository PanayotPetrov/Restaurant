namespace Restaurant.Web.ViewModels.Paging.PagedItemsModelCreator
{
    using System.Collections.Generic;

    using Restaurant.Web.ViewModels.Paging;

    public interface IPagedItemsModelCreator
    {
        public PagedItemsViewModel<T> Create<T>(IEnumerable<T> items, int pageNumber, int itemsPerPage, int totalItemCount);
    }
}
