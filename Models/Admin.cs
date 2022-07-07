using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public int? RoleId { get; set; }
        public string Fname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual Role? Role { get; set; }
    }
}
