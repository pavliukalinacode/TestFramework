using Application.Event.Service.Clients;

namespace Application.Event.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Clients
            builder.Services.AddHttpClient<ISubscriptionClient, SubscriptionClient>(client =>
            {
                client.BaseAddress = new Uri("http://subscription-service:8080");
            });

            builder.Services.AddHttpClient<IWebhookClient, WebhookClient>();

            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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