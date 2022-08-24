using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Answer
    {
        public int Id { get; set; }
        public string Structure { get; set; } = null!;
        public int FormId { get; set; }
        public int UserId { get; set; }
        public DateTime DateInserted { get; set; }

        public virtual Form Form { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
