using AbpTask.Data;
using AbpTask.Modules.Reservation.UseCases;
using AbpTask.Modules.Reservation.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Implementations;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ICreateRoomUseCase, CreateRoomUseCase>();
builder.Services.AddScoped<IUpdateRoomUseCase, UpdateRoomUseCase>();
builder.Services.AddScoped<IDeleteRoomUseCase, DeleteRoomUseCase>();
builder.Services.AddScoped<ISearchRoomsUseCase, SearchRoomsUseCase>();
builder.Services.AddScoped<ICreateReservationUseCase, CreateReservationUseCase>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
