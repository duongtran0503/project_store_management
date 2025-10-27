namespace StoreManagement.API
{
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();

            var app = builder.Build();

       
            app.ConfigureMiddleware();

      
            await app.SeedDataAsync();

   
            app.Run();
        }
    }
}
