namespace Restaurant.Web.HelperClasses
{
    using System.Collections.Generic;

    using Restaurant.Web.ViewModels;

    public interface IPagedItemsModelCreator
    {
        public PagedItemsListViewModel<T> Create<T>(IEnumerable<T> items, int pageNumber, int itemsPerPage, int totalItemCount);
    }
}
