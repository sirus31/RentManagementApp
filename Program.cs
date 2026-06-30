using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentManagementApp.Data;
using RentManagementApp.Services;
using RentManagementApp.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy
                .WithOrigins(
                    "https://rent-management-app-front-end-with.vercel.app",
                    "https://ghar-sewa-fawn.vercel.app/",
                    "http://localhost:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception("JWT Key is not configured.");
}

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions
            .Converters
            .Add(
                new JsonStringEnumConverter());
    });

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    ITenantService,
    TenantService>();

builder.Services.AddScoped<
    IHouseService,
    HouseService>();

builder.Services.AddScoped<
    IFloorService,
    FloorService>();

builder.Services.AddScoped<
    IRoomService,
    RoomService>();

builder.Services.AddScoped<
    IOccupancyService,
    OccupancyService>();

builder.Services.AddScoped<
    IMeterAssignmentService,
    MeterAssignmentService>();

builder.Services.AddScoped<
    IMeterReadingService,
    MeterReadingService>();

builder.Services.AddScoped<
    IBillService,
    BillService>();

builder.Services.AddScoped<
    IMeterService,
    MeterService>();

builder.Services.AddScoped<
    IPaymentService,
    PaymentService>();

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI();


app.Run();