using Microsoft.EntityFrameworkCore;
using RentManagementApp.Data;
using RentManagementApp.Services;
using RentManagementApp.Services.Interfaces;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();