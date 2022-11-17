using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class DriveCategory
    {
        public int Id { get; set; }
        public int DriveId { get; set; }
        public int CategoryId { get; set; }
        public string AllotedTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalQuestion { get; set; }
        public string QuestionIds { get; set; }

        public virtual Category Category { get; set; }
        public virtual Drive Drive { get; set; }
    }
}
