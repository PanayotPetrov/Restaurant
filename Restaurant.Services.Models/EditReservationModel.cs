namespace Restaurant.Services.Models
{
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class EditReservationModel : AddReservationModel, IMapTo<Reservation>
    {
        public string Id { get; set; }
    }
}
