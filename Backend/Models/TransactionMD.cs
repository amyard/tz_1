using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class TransactionMD
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public string ClientName { get; set; }
        public string Price { get; set; }
    }
}
