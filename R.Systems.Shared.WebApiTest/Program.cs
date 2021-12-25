using R.Systems.Shared.WebApi.Middlewares;
using R.Systems.Shared.WebApi.Serilog;
using R.Systems.Shared.WebApiTest.DependencyInjection;
using Serilog;

Log.Logger = SerilogConfiguration.CreateBootstrapLogger();
Log.Information("Starting up!");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UserSerilogWithStandardConfiguration();

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddServices(builder.Configuration);

    var app = builder.Build();

    app.UseSharedKernelExceptionHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
