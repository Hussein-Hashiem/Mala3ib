namespace Mala3ib.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();


        builder.Services.AddDependencies(builder.Configuration);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

        }
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));

        app.UseHangfireDashboard("/Jobs");

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");

        app.UseAuthorization();


        app.MapControllers();

        app.UseExceptionHandler();

        app.MapStaticAssets();

        app.Run();
    }
}
