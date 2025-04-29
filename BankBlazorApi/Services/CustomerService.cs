using Microsoft.EntityFrameworkCore;
using BankBlazorApi.Contexts;
using BankBlazorApi.Data;
using BankBlazorApi.Enums;
using BankBlazorApi.Services.Interfaces;

namespace BankBlazorApi.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly BankBlazorContext _dbContext;

        public CustomerService(BankBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var allCustomers = await _dbContext.Customers.ToListAsync();
            return allCustomers;
        }
        public async Task<Customer> GetCustomer(int id)
        {
            var customer = await _dbContext.Customers
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            return customer;
        }
    }
}
