namespace Backend.RequestModels
{
    public class TransactionFilterModel
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public int Page { get; set; }
    }
}