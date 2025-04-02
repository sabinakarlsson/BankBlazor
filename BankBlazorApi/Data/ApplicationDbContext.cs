using Microsoft.EntityFrameworkCore;

namespace BankBlazorApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
