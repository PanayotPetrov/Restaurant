using System.Collections.Generic;

namespace Restaurant.Services.Models
{
    public class AddAddressModel
    {
        public string Name { get; set; }

        public string Street { get; set; }

        public string AddressLineTwo { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }

        public bool IsPrimaryAddress { get; set; }
    }
}
