namespace Restaurant.Web.HelperClasses
{
    using System.Collections.Generic;
    using System.Linq;

    using Restaurant.Web.ViewModels;

    public class PagedItemsModelCreator : IPagedItemsModelCreator
    {
        public PagedItemsListViewModel<T> Create<T>(IEnumerable<T> items, int pageNumber, int itemsPerPage, int totalItemCount)
        {
            var model = new PagedItemsListViewModel<T>
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
