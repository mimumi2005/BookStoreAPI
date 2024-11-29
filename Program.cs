using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Entity framework, with sqlite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlite(connectionString));

// Adding Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(@"bin\Debug\net9.0\BookStoreAPI.xml");
});

var app = builder.Build();


// Enable Swagger UI for development environment
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
