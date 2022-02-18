namespace Restaurant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class Table : BaseDeletableModel<int>
    {
        public Table()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        [Range(2, 6)]
        public int NumberOfPeople { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public override bool Equals(object obj)
        {
            var table = obj as Table;
            if (this.Id == table.Id)
            {
                return true;
            }

            return false;
        }
    }
}
