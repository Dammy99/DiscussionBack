
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.Extensions.Options;

namespace DiscussionBackend
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
            //builder.Services.AddSingleton();
            IFirebaseConfig config = new FirebaseConfig()
            {
                AuthSecret = "b9ae053510e05c081310943f6f9057c7ede7ff9c659e5ddc4597a213553c4d65",
                BasePath = "https://discussion-1af24-default-rtdb.europe-west1.firebasedatabase.app/"
            };
            IFirebaseClient client;
            client = new FireSharp.FirebaseClient(config);

            builder.Services.AddSingleton<IFirebaseClient>(client);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            //app.UseCors(options => options.WithOrigins("http://localhost:5173/")
            //                  .AllowAnyMethod()
            //                  .AllowAnyHeader());
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) 
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        //public void LoadConfiguration()
        //{
        //    IConfiguration config = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .Build();

        //    // Отримання значень з конфігурації
        //    string apiKey = config.GetConnectionString("ApiKey"); // Значення параметра з ключем "Key" з файлу appsettings.json
        //    Console.WriteLine(apiKey);
        //    var te = new FirebaseAuthConfig();
        //    //var ka = FirebaseApp.Create();
        //    FirebaseAuthProvider provider = AuthProvider 
        //    //FirebaseAuthProvider authProvider = new(te);

        //}
    }
}
