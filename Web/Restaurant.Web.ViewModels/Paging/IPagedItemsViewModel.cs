namespace Restaurant.Web.ViewModels.Paging
{
    using System.Collections.Generic;

    public interface IPagedItemsViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public bool HasValidState { get; set; }
    }
}
