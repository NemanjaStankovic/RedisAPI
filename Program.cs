using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RedisAPI;
using RedisAPI.Data;
using RedisAPI.Models;
using SignalRChat.Hubs;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(p => 
    p.UseSqlServer(builder.Configuration.GetConnectionString("RedisApiCS"))); 


builder.Services.AddSingleton<IConnectionMultiplexer>(options => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));

    
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/User/Login";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", builder =>
    {
        builder.WithOrigins(new string[]
            {
                "https://127.0.0.1:7051",
                "https://localhost:7051",
                "http://127.0.0.1:7051",
                "http://localhost:7051"
            })
            .AllowAnyHeader()
            .WithMethods("GET", "POST")
            .AllowCredentials()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IPlatformRepo, RedisPlatfromRepo>();
builder.Services.AddHostedService<BackgroundService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddRazorPages();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("CORS");
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();
app.MapHub<PlatformHub>("/platformHub");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
