using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using server.Services; // Ensure this is included to reference your JwtService

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register services
builder.Services.AddControllers(); // Add this line for Controller support
builder.Services.AddSwaggerGen(); // For OpenAPI/Swagger documentation

// Register JwtService (Fix the location of this registration)
builder.Services.AddScoped<JwtService>(); // or AddSingleton/AddTransient based on your needs

// JWT configuration
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

if (jwtConfig == null || string.IsNullOrEmpty(jwtConfig.Key))
{
    throw new ArgumentNullException("JWT configuration or key is not provided.");
}

// Configure JWT authentication
builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Enable for development
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Display Swagger UI
    app.MapOpenApi();  // Expose OpenAPI docs
}

app.UseHttpsRedirection(); // Enforce HTTPS
app.UseCors("AllowAll"); // Enable CORS
app.UseAuthentication(); // Middleware for Authentication
app.UseAuthorization(); // Middleware for Authorization

app.MapControllers(); // Map controllers to handle requests

app.Run(); // Start the application
