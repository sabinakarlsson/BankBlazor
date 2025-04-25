using BankBlazorApi.Data;
using BankBlazorApi.Enums;


namespace BankBlazorApi.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllTransactions();

        Task<Account> GetAccount(int accountId);

        Task<Transaction> GetTransaction(int id);

        Task<ResponseCode> Insert(decimal amount, int accountId);

        Task<ResponseCode> Withdraw(decimal amount, int accountId);

        Task<ResponseCode> Transfer(int fromAccountId, int toAccountId, decimal amountToTransfer);


    }
}
