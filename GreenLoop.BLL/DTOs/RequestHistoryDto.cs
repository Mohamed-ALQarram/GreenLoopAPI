namespace GreenLoop.BLL.DTOs
{
    public class RequestHistoryDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AddressLabel { get; set; }
    }
}
