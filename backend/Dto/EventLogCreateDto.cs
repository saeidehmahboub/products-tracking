namespace Backend.Dto
{
    public class EventLogCreateDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Event { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}