using EmployeePortal.Data;
using EmployeePortal.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PortalDbContext>(options =>
    options.UseSqlite("Data Source=portal.db"));

builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<LegacyBonusCalculator>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();
