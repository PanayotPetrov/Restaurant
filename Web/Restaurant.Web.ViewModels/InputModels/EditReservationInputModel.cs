namespace Restaurant.Web.ViewModels.InputModels
{
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class EditReservationInputModel : AddReservationInputModel, IMapFrom<Reservation>, IMapTo<EditReservationModel>
    {
        public string Id { get; set; }
    }
}
