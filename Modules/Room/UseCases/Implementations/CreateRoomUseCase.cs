using AbpTask.Data;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Outputs;
using AbpTask.Data.Entities;
using AutoMapper;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class CreateRoomUseCase : ICreateRoomUseCase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRoomUseCase(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return _mapper.Map<CreateRoomUseCaseOutput>(roomEntity);
        }
    }
}
