using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public int? RoleId { get; set; }
        public string Fname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Sex { get; set; }
        public string? Caddress { get; set; }
        public string? CaddressCity { get; set; }
        public string? CaddressNumber { get; set; }
        public string? CaddressPost { get; set; }
        public string? Phone { get; set; }
        public int StudentNumber { get; set; } 
        public virtual Role? Role { get; set; }
    }
}
