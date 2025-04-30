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

        public async Task<Account> GetAccount(int accountId)
        {
            var account = await _dbContext.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            return account;
        }


        public async Task<ResponseCode> Insert(decimal amount, int accountId)
        {
            try
            {
                var account = await _dbContext.Accounts.FindAsync(accountId);

                if (account == null)
                {
                    return ResponseCode.NotFound;
                }

                account.Balance += amount;

                var transaction = new Transaction
                {
                    Amount = amount,
                    AccountId = accountId,
                    Date = DateOnly.FromDateTime(DateTime.Today)
                };

                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();
                return ResponseCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during transaction");
                return ResponseCode.InternalServerError;
            }
        }

        public async Task<ResponseCode> Withdraw(decimal amount, int accountId)
        {
            try
            {
                var account = await _dbContext.Accounts.FindAsync(accountId);
                if (account == null)
                {
                    return ResponseCode.NotFound;
                }
                if (account.Balance < amount)
                {
                    return ResponseCode.Forbidden;
                }
                account.Balance -= amount;

                var transaction = new Transaction
                {
                    Amount = amount,
                    AccountId = accountId,
                    Date = DateOnly.FromDateTime(DateTime.Today)
                };

                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();
                return ResponseCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during transaction");
                return ResponseCode.InternalServerError;
            }
        }
        public async Task<ResponseCode> Transfer(int fromAccountId, int toAccountId, decimal amountToTransfer)
        {
            fromAccountId = 1;

            try
            {
                var fromAccount = await _dbContext.Accounts.FindAsync(fromAccountId);
                var toAccount = await _dbContext.Accounts.FindAsync(toAccountId);
                if (fromAccount == null || toAccount == null)
                {
                    Console.WriteLine("One or both of the accounts do not exist.");
                    return ResponseCode.NotFound;
                }
                if (fromAccount.Balance < amountToTransfer)
                {
                    Console.WriteLine("The amount you are trying to transfer exceeds the account balance.");
                    return ResponseCode.Forbidden;
                }
                fromAccount.Balance -= amountToTransfer;
                toAccount.Balance += amountToTransfer;

                var withdrawalTransaction = new Transaction
                {
                    AccountId = fromAccountId,
                    Amount = amountToTransfer,
                    Date = DateOnly.FromDateTime(DateTime.Today)
                };

                // Skapa transaktion för insättning
                var depositTransaction = new Transaction
                {
                    AccountId = toAccountId,
                    Amount = amountToTransfer,
                    Date = DateOnly.FromDateTime(DateTime.Today)
                };

                await _dbContext.Transactions.AddRangeAsync(withdrawalTransaction, depositTransaction);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("The transfer was successful");
                return ResponseCode.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during transaction");
                Console.WriteLine(ex.Message.ToString());
                return ResponseCode.InternalServerError;
            }
        }
    }

}
