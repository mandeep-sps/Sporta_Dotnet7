using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class Question
    {
        public int Id { get; set; }
        public string Question1 { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string QuestionLevel { get; set; }

        public virtual Category Category { get; set; }
    }
}
