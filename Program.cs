using AbpTask.Modules.Room.UseCases.Implementations;
using AbpTask.Modules.Room.UseCases.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICreateRoomUseCase, CreateRoomUseCase>();

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
