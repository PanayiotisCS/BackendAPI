namespace BackendAPI.Models
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
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
    }
}
