namespace BackendAPI.Requests
{
    public class AnswerDto
    {
        public string Structure { get; set; } = null!;
        public int FormId { get; set; }
        public int UserId { get; set; }
        public DateTime DateInserted { get; set; }
    }
}
