using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this line for Controller support
builder.Services.AddSwaggerGen(); // For OpenAPI/Swagger documentation

var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

if (jwtConfig == null || string.IsNullOrEmpty(jwtConfig.Key))
{
    throw new ArgumentNullException("JWT configuration or key is not provided.");
}


builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>{
    options.RequireHttpsMetadata = false; //enable for development
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters{
    ValidIssuer = jwtConfig.Issuer,
    ValidAudience = jwtConfig.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
    ValidateIssuer =  true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    };
}); 

builder.Services.AddAuthorization();

// builder.Services.AddSingleton<MySqlConnectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // Expose OpenAPI/Swagger docs in development
}

app.UseHttpsRedirection();  // Enforce HTTPS

//middlewares for auth
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();  // Map controllers to handle requests

app.Run();
