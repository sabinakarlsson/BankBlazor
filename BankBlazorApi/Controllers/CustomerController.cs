using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankBlazorApi.DTOs;
using BankBlazorApi.Services.Interfaces;

namespace BankBlazorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerReadDTO>>> GetAll()
        {
            var customerEntities = await _customerService.GetAllCustomers();
            if (customerEntities == null)
            {
                return NotFound();
            }
            var customerDtos = customerEntities.Select(customerEntity => new CustomerReadDTO
            {
                CustomerId = Convert.ToInt32(customerEntity.CustomerId),
                Givenname = customerEntity.Givenname,
                Surname = customerEntity.Surname,

            });
            return Ok(customerDtos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReadDTO>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            var disposition = customer.Dispositions.FirstOrDefault();
            var accountId = disposition?.AccountId;
            var balance = disposition?.Account?.Balance;

            var customerDto = new CustomerReadDTO
            {
                CustomerId = Convert.ToInt32(customer.CustomerId),
                Givenname = customer.Givenname,
                Surname = customer.Surname,
                AccountId = accountId,
                Balance = balance
            };
            return Ok(customerDto);
        }
    }
}
