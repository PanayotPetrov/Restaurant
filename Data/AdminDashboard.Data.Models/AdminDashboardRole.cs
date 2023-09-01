namespace AdminDashboard.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;
    using Restaurant.Data.Common.Models;

    public class AdminDashboardRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public AdminDashboardRole()
            : this(null)
        {
        }

        public AdminDashboardRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
