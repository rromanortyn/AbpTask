using AbpTask.Data;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Outputs;
using AbpTask.Data.Entities;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class CreateRoomUseCase : ICreateRoomUseCase
    {
        private readonly ApplicationDbContext _context;

        public CreateRoomUseCase(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateRoomUseCaseOutput> Execute(CreateRoomUseCaseInput input)
        {
            var servicesEntities = input.Services
                .Select((service) =>
                {
                    return new ServiceEntity
                    {
                        Name = service.Name,
                        Price = service.Price,
                    };
                })
                .ToList();

            var roomEntity = new RoomEntity
            {
                Name = input.Name,
                Capacity = input.Capacity,
                BasePricePerHour = input.BasePricePerHour,
                Services = servicesEntities,
            };

            await _context.Rooms.AddAsync(roomEntity);
            await _context.SaveChangesAsync();

            return new CreateRoomUseCaseOutput
            {
                Id = roomEntity.Id,
                Name = roomEntity.Name,
                Capacity = roomEntity.Capacity,
                BasePricePerHour = roomEntity.BasePricePerHour,
                Services = [.. roomEntity.Services.Select((service) =>
                {
                    return new CreateRoomUseCaseOutput.Service
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Price = service.Price,
                        CreatedAt = service.CreatedAt,
                        UpdatedAt = service.UpdatedAt,
                    };
                })],
                CreatedAt = roomEntity.CreatedAt,
                UpdatedAt = roomEntity.UpdatedAt,
            };
        }
    }
}
