namespace Restaurant.Services.Data.Tests
{
    using System.Reflection;

    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels;

    public class MapperAssemblyFixture
    {
        public MapperAssemblyFixture()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(AddReservationModel).GetTypeInfo().Assembly);
        }
    }
}
