namespace Backend.Dto
{
    public class EventLogDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Event { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}