using AbpTask.Data;
using AbpTask.Modules.Reservation.UseCases;
using AbpTask.Modules.Reservation.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Implementations;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Shared;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

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

builder.Services
    .AddControllers(options =>
    {
        options.Conventions.Add(
            new RouteTokenTransformerConvention(
                new LowercaseParameterTransformer()
            )
        );
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling =
        JsonNumberHandling.Strict;
});

builder.Services.AddOpenApi(options =>
{
    options.CreateSchemaReferenceId = jsonTypeInfo =>
    {
        var type = jsonTypeInfo.Type;

        if (type.IsNested && type.DeclaringType != null)
        {
            return $"{type.DeclaringType.Name}_{type.Name}";
        }

        return OpenApiOptions.CreateDefaultSchemaReferenceId(jsonTypeInfo);
    };
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
