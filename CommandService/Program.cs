using CommandService.Data;
using CommandService.EventProcessing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SQL Server Db");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(config.GetConnectionString("CommandsConnection"));
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Populate();

app.Run();
