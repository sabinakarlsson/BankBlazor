using BankBlazorApi.Data;
using BankBlazorApi.Enums;


namespace BankBlazorApi.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllTransactions();

        Task<Transaction> GetTransaction(int id);

        Task<ResponseCode> Insert(Transaction transaction);

        Task<ResponseCode> Withdraw(Transaction transaction);

        Task<ResponseCode> Transfer(int fromAccountId, int toAccountId, decimal amountToTransfer);


    }
}
