namespace SportProf.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public string Type { get; set; } = "alert-success";
    }
}