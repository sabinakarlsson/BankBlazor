using System.ComponentModel.DataAnnotations;

namespace BankBlazorApi.DTOs
{
    public class TransferDTO
    {
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }
    }
}
