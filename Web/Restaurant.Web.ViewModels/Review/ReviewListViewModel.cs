namespace Restaurant.Web.ViewModels.Review
{
    using System.Collections.Generic;

    using Restaurant.Web.ViewModels.Paging;

    public class ReviewListViewModel : PagingViewModel
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
