using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class Candidate
    {
        public Candidate()
        {
            Results = new HashSet<Result>();
        }

        public int Id { get; set; }
        public string ContactNumber { get; set; }
        public int CandidateStatusId { get; set; }
        public int DriveId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string RollNo { get; set; }
        public string Branch { get; set; }

        public virtual CandidateStatus CandidateStatus { get; set; }
        public virtual Drive Drive { get; set; }
        public virtual ApplicationUser IdNavigation { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
