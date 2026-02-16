using GreenLoop.DAL.Enums;

namespace GreenLoop.BLL.DTOs
{
    public class WalletBalanceDto
    {
        public int CurrentBalance { get; set; }
        public int TotalEarned { get; set; }
    }

    public class CouponDto
    {
        public int CouponId { get; set; }
        public string Title { get; set; }
        public string PartnerName { get; set; }
        public int RequiredPoints { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class RedeemRequestDto
    {
        public int CouponId { get; set; }
    }

    public class RedeemResponseDto
    {
        public string CouponCode { get; set; }
        public int RemainingBalance { get; set; }
    }

    public class TransactionDto
    {
        public string Type { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
