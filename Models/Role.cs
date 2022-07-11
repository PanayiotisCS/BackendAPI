using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            Admins = new HashSet<Admin>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public int? User_Id { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual User? User { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
