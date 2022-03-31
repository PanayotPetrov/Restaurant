namespace Restaurant.Web.ViewModels.InputModels
{
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class CreateOrderInputModel
    {
        [CartIdValidation]
        public int CartId { get; set; }

        [AddressNameValidation]
        public string AddressName { get; set; }
    }
}
