using Microsoft.EntityFrameworkCore;
using RentManagementApp.Data;
using RentManagementApp.Services;
using RentManagementApp.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

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

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();