using System;
using System.Collections.Generic;

namespace BackendAPI.Models
{
    public partial class User
    {
        public User()
        {
            Admins = new HashSet<Admin>();
            RefreshTokens = new HashSet<RefreshToken>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
