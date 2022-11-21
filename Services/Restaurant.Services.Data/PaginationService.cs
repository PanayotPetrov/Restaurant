﻿namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Models;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Services.Mapping;

    public class PaginationService<T> : IPaginationService
        where T : class, IDeletableEntity, IAuditInfo
    {
        private readonly IDeletableEntityRepository<T> repository;

        public PaginationService(IDeletableEntityRepository<T> repository)
        {
            this.repository = repository;
        }

        public int GetCount(bool getDeleted = false)
        {
            return getDeleted == true
                ? this.repository.AllAsNoTrackingWithDeleted().Count()
                : this.repository.AllAsNoTracking().Count();
        }

        public IEnumerable<T2> GetAllWithPagination<T2>(int itemsPerPage, int page, bool getDeleted = false)
        {
            if (getDeleted)
            {
                return this.repository.AllAsNoTrackingWithDeleted()
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .To<T2>()
                    .ToList();
            }

            return this.repository.AllAsNoTracking()
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip((page - 1) * itemsPerPage)
                 .Take(itemsPerPage)
                 .To<T2>()
                 .ToList();
        }
    }
}
