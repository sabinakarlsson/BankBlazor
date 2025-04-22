using Microsoft.EntityFrameworkCore;
using BankBlazorApi.Contexts;
using BankBlazorApi.Data;
using BankBlazorApi.Enums;
using BankBlazorApi.Services.Interfaces;

namespace BankBlazorApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankBlazorContext _dbContext;

        public TransactionService(BankBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            var allTransactions = await _dbContext.Transactions.ToListAsync();

            return allTransactions;
        }

        public async Task<Transaction> GetTransaction(int id)
        {
            var transaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == id);
            if (transaction == null)
            {
                throw new Exception("Transaction not found");
            }

            return transaction;
        }
        public async Task<ResponseCode> Insert(Transaction transaction)
        {
            try
            {
                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();
                return ResponseCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel inträffade vid överföringen");
                return ResponseCode.InternalServerError;
            }
        }

        public async Task<ResponseCode> Withdraw(Transaction transaction)
        {
            try
            {
                var account = await _dbContext.Accounts.FindAsync(transaction.AccountId);
                if (account == null)
                {
                    return ResponseCode.NotFound;
                }
                if (account.Balance < transaction.Amount)
                {
                    return ResponseCode.Forbidden;
                }
                account.Balance -= transaction.Amount;
                await _dbContext.SaveChangesAsync();
                return ResponseCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel inträffade vid överföringen");
                return ResponseCode.InternalServerError;
            }
        }
        public async Task<ResponseCode> Transfer(int fromAccountId, int toAccountId, decimal amountToTransfer)
        {
            try
            {
                var fromAccount = await _dbContext.Accounts.FindAsync(fromAccountId);
                var toAccount = await _dbContext.Accounts.FindAsync(toAccountId);
                if (fromAccount == null || toAccount == null)
                {
                    Console.WriteLine("Ett eller fler av kontona existerar inte.");
                    return ResponseCode.NotFound;
                }
                if (fromAccount.Balance < amountToTransfer)
                {
                    Console.WriteLine("Summan du försöker föra över överstiger saldot på kontot.");
                    return ResponseCode.Forbidden;
                }
                fromAccount.Balance -= amountToTransfer;
                toAccount.Balance += amountToTransfer;
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Överföringen lyckades");
                return ResponseCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel inträffade vid överföringen");
                return ResponseCode.InternalServerError;
            }
        }
    }

}
