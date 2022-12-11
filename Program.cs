// using Employees.API.Data;
// using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(setup=> setup.Configuration = builder.Configuration.GetConnectionString("RedisCache"));

// builder.Services.AddDbContext<EmployeesDbContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeesContext")));

builder.Services.AddCompositionRoot();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseCors(builder => 
//     builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
// );

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
