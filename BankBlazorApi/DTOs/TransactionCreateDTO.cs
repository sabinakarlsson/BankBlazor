namespace BankBlazorApi.DTOs
{
    public class TransactionCreateDTO
    {
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
