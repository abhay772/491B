using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using AA.PMTOGO;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebApp1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApp1Context") ?? throw new InvalidOperationException("Connection string 'WebApp1Context' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
//implement CORS
//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
