using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Controllers
builder.Services.AddControllers();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtService>();

var app = builder.Build();



app.UseCors("AllowFrontend");

app.UseDeveloperExceptionPage();

// Middleware
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Создание admin
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    if (!db.Users.Any())
//    {
//        var admin = new User
//        {
//            Login = "admin",
//            PasswordHash = Convert.ToBase64String(
//        System.Security.Cryptography.SHA256.HashData(
//            Encoding.UTF8.GetBytes("admin123")
//        )
//    ),

//            PreviousPasswordHash = "",
//            ParentPasswordHash = "", // 👈 ВАЖНО

//            Role = "Admin",
//            Nickname = "Admin",
//            Age = 18,
//            Theme = "dark",
//            AvatarUrl = "",
//            Status = "Online",
//            LastOnline = DateTime.Now
//        };

//        db.Users.Add(admin);
//        db.SaveChanges();
//    }
//}

app.Run();