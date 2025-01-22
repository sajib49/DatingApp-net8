using API.Extentions;
using API.Midlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAccountServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
//app.UseDeveloperExceptionPage();
app.UseMiddleware<ExceptionMidleware>();
app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
