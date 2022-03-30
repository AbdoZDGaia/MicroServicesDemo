using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SQL Server Db");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(config.GetConnectionString("PlatformsConnection"));
    });
}
else
{
    Console.WriteLine("--> Using In Memory Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseInMemoryDatabase("Db");
    });
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Populate();

app.Run();
