using Microsoft.EntityFrameworkCore;
//using PMTOGO.WebAPP.Data;
using AA.PMTOGO.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PMTOGO.WebAPP.Controllers;
using PMTOGO.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();

//builder.Services.AddDbContext<UserManagementController>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDbConnectionString")));



//builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDbConnectionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
