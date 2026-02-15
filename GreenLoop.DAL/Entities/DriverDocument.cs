using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class DriverDocument
    {
        [Key]
        public int Id { get; set; }

        // ربط المستند بالسائق فقط (Strict Typing)
        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        // Navigation Property من نوع Driver مش User
        // عشان نضمن إن المستندات دي للسواقين بس
        public Driver Driver { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocumentType { get; set; } // "License", "NationalID_Front", "NationalID_Back", "CriminalRecord"

        [Required]
        public string ImageUrl { get; set; } // مسار الصورة على السيرفر أو رابط الـ Blob Storage

        public bool IsApproved { get; set; } = false; // هل الأدمن وافق عليها؟

        [MaxLength(200)]
        public string? RejectionReason { get; set; } // لو اترفضت، ليه؟

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
