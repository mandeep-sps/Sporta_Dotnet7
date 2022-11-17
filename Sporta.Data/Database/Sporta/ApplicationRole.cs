using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class ApplicationRole
    {
        public ApplicationRole()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
