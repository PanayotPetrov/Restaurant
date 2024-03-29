﻿namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Restaurant.Data.Common.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Mapping;

    public abstract class PaginationService<T> : IPaginationService
        where T : class, IDeletableEntity, IAuditInfo
    {
        private readonly IRestaurantDeletableEntityRepositoryDecorator<T> repository;

        public PaginationService(IRestaurantDeletableEntityRepositoryDecorator<T> repository)
        {
            this.repository = repository;
        }

        public virtual int GetCount(bool getDeleted = false)
        {
            return getDeleted == true
                ? this.repository.AllAsNoTrackingWithDeleted().Count()
                : this.repository.AllAsNoTracking().Count();
        }

        public virtual IEnumerable<T2> GetAllWithPagination<T2>(int itemsPerPage, int page, bool getDeleted = false)
        {
            var itemsToSkip = this.GetItemsToSkip(itemsPerPage, page, getDeleted);

            if (getDeleted)
            {
                return this.repository.AllAsNoTrackingWithDeleted()
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip(itemsToSkip)
                    .Take(itemsPerPage)
                    .To<T2>()
                    .ToList();
            }

            return this.repository.AllAsNoTracking()
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(itemsToSkip)
                 .Take(itemsPerPage)
                 .To<T2>()
                 .ToList();
        }

        protected int GetItemsToSkip(int itemsPerPage, int page, bool getDeleted)
        {
            var itemsToSkip = Math.Abs(page - 1) * itemsPerPage;

            if (itemsToSkip > this.GetCount(getDeleted))
            {
                itemsToSkip = 0;
            }

            return itemsToSkip;
        }
    }
}
