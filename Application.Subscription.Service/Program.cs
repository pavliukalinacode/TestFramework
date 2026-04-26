using Application.Subscription.Service.Data;
using Application.Subscription.Service.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Subscription.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DB
            builder.Services.AddDbContext<NotificationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DI
            builder.Services.AddScoped<ISubscriptionRepository, SqlSubscriptionRepository>();

            var app = builder.Build();

            // Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}