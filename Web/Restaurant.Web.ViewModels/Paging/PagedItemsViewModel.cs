﻿namespace Restaurant.Web.ViewModels.Paging
{
    using System.Collections.Generic;

    public class PagedItemsViewModel<T> : PagingViewModel, IPagedItemsViewModel<T>
    {
        public virtual IEnumerable<T> Items { get; set; }

        public virtual bool HasValidState { get; set; }
    }
}
