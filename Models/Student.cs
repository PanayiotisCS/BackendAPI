﻿using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Fname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int StudentNumber { get; set; }
        public string? Sex { get; set; }
        public string? Caddress { get; set; }
        public string? CaddressCity { get; set; }
        public string? CaddressNumber { get; set; }
        public string? CaddressPost { get; set; }
        public string? Phone { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
