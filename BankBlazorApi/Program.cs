
using Microsoft.EntityFrameworkCore;
using BankBlazorApi.Contexts;
using BankBlazorApi.Services;
using BankBlazorApi.Services.Interfaces;
using BankBlazorApi.Data;

namespace BankBlazorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /*Initiera NorthwindContext(dbContext) för Dependency Injection*/
            builder.Services.AddDbContext<BankBlazorContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindContext"));
            });

            builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseCors("AllowBlazorClient");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
