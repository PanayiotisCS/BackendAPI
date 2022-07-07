using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; }
    }
}
