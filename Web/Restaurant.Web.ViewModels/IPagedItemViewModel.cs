namespace Restaurant.Web.ViewModels
{
    using System.Collections.Generic;

    public interface IPagedItemsListViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public bool HasValidState { get; set; }
    }
}
