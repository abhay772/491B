using AA.PMTOGO.WebAPP.Data;
using AA.PMTOGO.Infrastructure;
using AA.PMTOGO.Infrastructure.JSONConverters;
using Microsoft.EntityFrameworkCore;
using AA.PMTOGO.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("UsersDbConnectionString"),
        b => b.MigrationsAssembly("AA.PMTOGO.WebAPP")
    )
);
builder.Services.AddDbContext<ServiceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceDbConnectionString")));

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
        });

var app = builder.Build();

app.UseMiddleware<CorsMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
