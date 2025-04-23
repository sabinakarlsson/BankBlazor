namespace BankBlazor.Client.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Givenname { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
    }
}
