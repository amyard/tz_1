using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class TransactionMD
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public Type Type { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
