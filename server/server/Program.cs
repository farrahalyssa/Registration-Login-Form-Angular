using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this line for Controller support
builder.Services.AddSwaggerGen(); // For OpenAPI/Swagger documentation
builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>{
    options.RequireHttpsMetadata = false; //enable for development
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters{
    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
    ValidAudience = builder.Configuration["JwtConfig:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
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
