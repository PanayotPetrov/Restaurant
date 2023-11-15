namespace Restaurant.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Restaurant.Data.Common;
    using Restaurant.Data.Models;

    internal class TableSeeder : ISeeder<ApplicationDbContext>
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Tables.Any())
            {
                return;
            }

            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 2,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 2,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 4,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 4,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 4,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 4,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 6,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 6,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 6,
            });
            await dbContext.Tables.AddAsync(new Table
            {
                NumberOfPeople = 6,
            });
        }
    }
}
