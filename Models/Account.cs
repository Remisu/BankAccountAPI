namespace BankAccountAPI.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Entity { get; set; }
        public DateTime OpeningDate { get; set; }
        public List<string> TransferHistory { get; set; }
        public string AccountStatus { get; set; }
    }
}
