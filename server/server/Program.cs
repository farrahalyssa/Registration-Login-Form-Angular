var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this line for Controller support
builder.Services.AddOpenApi(); // For OpenAPI/Swagger documentation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // Expose OpenAPI/Swagger docs in development
}

app.UseHttpsRedirection();  // Enforce HTTPS

app.MapControllers();  // Map controllers to handle requests

app.Run();
