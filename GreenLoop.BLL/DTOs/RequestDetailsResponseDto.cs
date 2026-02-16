namespace GreenLoop.BLL.DTOs
{
    public class RequestDetailsResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? CustomerNotes { get; set; }
        public int TotalPointsEarned { get; set; }

        // Address Info
        public RequestAddressDto Address { get; set; } = null!;

        // Request Items
        public List<RequestDetailItemDto> Items { get; set; } = new();
    }

    public class RequestAddressDto
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Governorate { get; set; }
        public string City { get; set; } = string.Empty;
        public string? StreetName { get; set; }
    }

    public class RequestDetailItemDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryIcon { get; set; }
        public decimal EstimatedWeight { get; set; }
        public decimal ActualWeight { get; set; }
        public int PointsEarned { get; set; }
    }
}
