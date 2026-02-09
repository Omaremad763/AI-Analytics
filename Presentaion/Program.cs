
using System.Text.Json;
using Hangfire;

using Infrastructure.Extentions;
using Infrastructure.Persistence;
WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);
WebApplication? app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHangfireDashboard("/HangfireAnalytics");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
