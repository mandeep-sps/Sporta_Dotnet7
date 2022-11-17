using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class CandidateStatus
    {
        public CandidateStatus()
        {
            Candidates = new HashSet<Candidate>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
