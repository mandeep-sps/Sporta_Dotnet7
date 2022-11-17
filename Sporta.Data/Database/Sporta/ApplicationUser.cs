using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            DriveAssignees = new HashSet<DriveAssignee>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public int ApplicationRoleId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual ICollection<DriveAssignee> DriveAssignees { get; set; }
    }
}
