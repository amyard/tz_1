using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class TransactionMD
    {
        [Key]
        public int TransactionId { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public string ClientName { get; set; }
        public string Amount { get; set; }
    }
}
