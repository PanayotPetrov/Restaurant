namespace Restaurant.Web.ViewModels.Review
{
    using System.Collections.Generic;

    public class AdminReviewListViewModel : PagingViewModel
    {
        public IEnumerable<AdminReviewViewModel> Reviews { get; set; }
    }
}
