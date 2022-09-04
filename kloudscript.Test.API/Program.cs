using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Middleware;
using kloudscript.Test.API.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.RegisterServices();

var cosmosConfig = builder.Configuration.GetSection("AppSettings").Get<AppSettingsEntity>();

if (cosmosConfig != null)
{
    builder.Services.Configure<AppSettingsEntity>(builder.Configuration.GetSection("AppSettings"));
}

var app = builder.Build();

app.ConfigureSwagger();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.Run();
