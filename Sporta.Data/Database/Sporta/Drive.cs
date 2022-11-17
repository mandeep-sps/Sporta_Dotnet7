using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class Drive
    {
        public Drive()
        {
            Candidates = new HashSet<Candidate>();
            DriveAssignees = new HashSet<DriveAssignee>();
            DriveCategories = new HashSet<DriveCategory>();
        }

        public int Id { get; set; }
        public string DriveName { get; set; }
        public DateTime Scheduled { get; set; }
        public int Enrolled { get; set; }
        public int Token { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<DriveAssignee> DriveAssignees { get; set; }
        public virtual ICollection<DriveCategory> DriveCategories { get; set; }
    }
}
