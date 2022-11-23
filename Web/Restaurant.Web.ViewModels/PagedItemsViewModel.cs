namespace Restaurant.Web.ViewModels
{
    using System.Collections.Generic;

    public class PagedItemsListViewModel<T> : PagingViewModel, IPagedItemsListViewModel<T>
    {
        public virtual IEnumerable<T> Items { get; set; }

        public virtual bool HasValidState { get; set; }
    }
}
