using BankBlazorApi.DTOs;
using BankBlazorApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BankBlazorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        [HttpGet]
        public async Task<ActionResult<List<TransactionReadDTO>>> GetAll()
        {
            var transactionEntities = await _transactionService.GetAllTransactions();

            if (transactionEntities == null)
            {
                return NotFound();
            }

            var transactionDtos = transactionEntities.Select(transactionEntity => new TransactionReadDTO
            {
                AccountId = Convert.ToInt32(transactionEntity.AccountId),
                Balance = Convert.ToDecimal(transactionEntity.Balance),
            });

            return Ok(transactionDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionReadDTO>> GetAccount(int id)
        {
            var account = await _transactionService.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            var transactionDto = new TransactionReadDTO
            {
                AccountId = Convert.ToInt32(account.AccountId),
                Balance = Convert.ToDecimal(account.Balance),
            };
            return Ok(transactionDto);
        }


        [HttpPost("insert")]
        public async Task<ActionResult<TransactionReadDTO>> InsertMoney(TransactionCreateDTO transactionDTO)
        {

            await _transactionService.Insert(transactionDTO.Amount, transactionDTO.AccountId);
            return null;
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<TransactionReadDTO>> WithdrawMoney(TransactionCreateDTO transactionDTO)
        {

            await _transactionService.Withdraw(transactionDTO.Amount, transactionDTO.AccountId);
            return null;
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransactionReadDTO>> TransferMoney(TransferDTO transferDTO)
        {
            await _transactionService.Transfer(transferDTO.FromAccountId, transferDTO.ToAccountId, transferDTO.Amount);
            return null;

        }



    }
}
