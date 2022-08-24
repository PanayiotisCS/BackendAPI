using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Form
    {
        public Form()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Structure { get; set; } = null!;

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
