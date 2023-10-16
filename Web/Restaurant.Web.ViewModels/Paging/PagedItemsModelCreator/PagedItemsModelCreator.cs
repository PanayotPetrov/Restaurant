namespace Restaurant.Web.ViewModels.Paging.PagedItemsModelCreator
{
    using System.Collections.Generic;
    using System.Linq;

    using Restaurant.Web.ViewModels.Paging;

    public class PagedItemsModelCreator : IPagedItemsModelCreator
    {
        public PagedItemsViewModel<T> Create<T>(IEnumerable<T> items, int pageNumber, int itemsPerPage, int totalItemCount)
        {
            var model = new PagedItemsViewModel<T>
            {
                Items = items,
                ItemsPerPage = itemsPerPage,
                ItemCount = totalItemCount,
                PageNumber = pageNumber,
                HasValidState = true,
            };

            if ((pageNumber > model.PagesCount && model.Items.Any()) || pageNumber < 1)
            {
                model.HasValidState = false;
            }

            if (!model.Items.Any())
            {
                model.PageNumber = 1;
            }

            return model;
        }
    }
}
