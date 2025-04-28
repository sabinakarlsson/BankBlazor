using BankBlazorApi.Data;
using BankBlazorApi.Enums;

namespace BankBlazorApi.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomer(int id);
    }
}
