namespace Restaurant.Data.Common
{

    using System;
    using System.IO;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public class DesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T>
        where T : DbContext
    {
        public DesignTimeDbContextFactory(string connectionStringName)
        {
            this.ConnectionStringName = connectionStringName;
        }

        protected string ConnectionStringName { get; }

        public T CreateDbContext(string[] args)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //    Debugger.Break();
            //}

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<T>();
            var connectionString = configuration.GetConnectionString(this.ConnectionStringName);
            builder.UseSqlServer(connectionString);

            var dbContext = Activator.CreateInstance(typeof(T), builder.Options) as T;

            return dbContext;
        }
    }
}
