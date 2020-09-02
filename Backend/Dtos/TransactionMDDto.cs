namespace Backend.Dtos
{
    public class TransactionMDDto
    {
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public string Amount { get; set; }
    }
}