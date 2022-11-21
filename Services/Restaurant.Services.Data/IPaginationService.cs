namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPaginationService
    {
        public int GetCount(bool getDeleted = false);

        public IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int id, bool getDeleted = false);
    }
}
