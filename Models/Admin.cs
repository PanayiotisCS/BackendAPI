using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Fname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
