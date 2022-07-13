using System.ComponentModel.DataAnnotations;
namespace BackendAPI.Requests
{
    public class SignupRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Fname { get; set; } = null!;
        [Required]
        public string Lname { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public int StudentNumber { get; set; }
        public string? Sex { get; set; }
        public string? Caddress { get; set; }
        public string? CaddressCity { get; set; }
        public string? CaddressNumber { get; set; }
        public string? CaddressPost { get; set; }
        public string? Phone { get; set; }

        [Required]
        public DateTime Ts { get; set; }
    }
}
