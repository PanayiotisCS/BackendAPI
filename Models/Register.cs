namespace BackendAPI.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Username { get; set; } 
        public string Password { get; set; }
        public string Address { get; set; }
        public string AddressCity { get; set; }
        public string AddressNumber { get; set; }  
        public string AddressPost { get; set; }
        public string Phone { get; set; }
        public int StudentNumber { get; set; }
        public string Email { get; set; }
    }
}
