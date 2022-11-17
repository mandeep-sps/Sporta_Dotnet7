using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class DriveAssignee
    {
        public int Id { get; set; }
        public int DriveId { get; set; }
        public int ApplicationUserId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Drive Drive { get; set; }
    }
}
