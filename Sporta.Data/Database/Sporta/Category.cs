using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class Category
    {
        public Category()
        {
            DriveCategories = new HashSet<DriveCategory>();
            Questions = new HashSet<Question>();
            ResultDetails = new HashSet<ResultDetail>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<DriveCategory> DriveCategories { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ResultDetail> ResultDetails { get; set; }
    }
}
