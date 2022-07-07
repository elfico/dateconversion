namespace DateTimeConversion.Models
{
    public class AccountTransaction
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public int TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}