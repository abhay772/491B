namespace AA.PMTOGO_v2;

public class Startup
{
    public IConfiguration _configuration { get; }

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
                builder
                .WithOrigins("http://127.0.0.1:8080")
                .WithMethods("GET")
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromHours(24))
                );

            options.AddPolicy("AllowPost", builder =>
            {
                builder
                .WithOrigins("http://127.0.0.1:8080")
                .WithMethods("POST")
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromHours(24));
            });

            options.AddPolicy("AllowPost", builder =>
            {
                builder
                .WithOrigins("http://127.0.0.1:8080")
                .WithMethods("POST")
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromHours(24));
            });

            options.AddPolicy("AllowPost", builder =>
            {
                builder
                .WithOrigins("http://127.0.0.1:8080")
                .WithMethods("POST")
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromHours(24));
            });
        });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.RunAsync();
    }
}
