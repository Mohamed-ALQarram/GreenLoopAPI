
namespace GreenLoop.BLL.DTOs
{
    public class DriverStatusDto
    {
        public string Status { get; set; } // "Available" | "Busy"
    }

    public class TaskCompleteDto
    {
        public decimal ActualWeight { get; set; }
        public List<int> WasteCategoryIds { get; set; } = new List<int>();

    }

    public class DriverTaskDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
    }
}
