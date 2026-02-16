using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenLoop.DAL.Enums;

namespace GreenLoop.DAL.Entities
{
    public class WalletTransaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int Amount { get; set; } // Points added or deducted

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
