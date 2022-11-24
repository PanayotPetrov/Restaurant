namespace Restaurant.Services.Data
{
    using System.Collections.Generic;

    public interface IPaginationService
    {
        public int GetCount(bool getDeleted = false);

        public IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int id, bool getDeleted = false);
    }
}
