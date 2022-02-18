namespace Restaurant.Services.Data
{
    using System;

    public interface ITableService
    {
        int GetAvailableTableId(DateTime date, int numberOfPeople);
    }
}
