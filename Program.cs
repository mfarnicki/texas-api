using Texas.API;
using Texas.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddCompositionRoot();
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(builder =>
    builder.WithOrigins(app.Configuration.GetValue<string>("AllowedOrigin")).AllowAnyHeader().AllowAnyMethod().AllowCredentials()
);

app.MapControllers();
app.MapHub<GameHub>("/gameHub");
app.MapHealthChecks("/health");

app.Run();
