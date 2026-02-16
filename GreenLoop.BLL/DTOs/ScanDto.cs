using System;

namespace GreenLoop.BLL.DTOs
{
    public class ScanRequestDto
    {
        public string QrCode { get; set; }
    }

    public class ScanResponseDto
    {
        public int CustomerId { get; set; }
        public int PointsAdded { get; set; }
        public int NewTotalPoints { get; set; }
        public string Message { get; set; }
    }

    public class QrTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
