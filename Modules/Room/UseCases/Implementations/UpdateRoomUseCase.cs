using AbpTask.Data;
using AbpTask.Data.Entities;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Outputs;
using AbpTask.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class UpdateRoomUseCase : IUpdateRoomUseCase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRoomUseCase(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateRoomUseCaseOutput> Execute(UpdateRoomUseCaseInput input)
        {
            var roomEntity = await _context.Rooms
                .Include(r => r.Services)
                .FirstOrDefaultAsync(r => r.Id == input.Id);

            if (roomEntity == null)
            {
                throw new AppException(
                    new AppException.Info
                    {
                        Type = AppException.Info.TypeEnum.NotFound,
                        Code = "RoomNotFound",
                        Message = $"Room with ID {input.Id} not found."
                    },
                    null
                );
            }

            if (input.HasName && input.Name != null)
            {
                roomEntity.Name = input.Name;
            }

            if (input.HasCapacity && input.Capacity.HasValue)
            {
                roomEntity.Capacity = input.Capacity.Value;
            }

            if (input.HasBasePricePerHour && input.BasePricePerHour.HasValue)
            {
                roomEntity.BasePricePerHour = input.BasePricePerHour.Value;
            }

            var servicesToAddEntities = input.ServicesToAdd.Select(service => new ServiceEntity
            {
                Name = service.Name,
                Price = service.Price,
            }).ToList();

            ((List<ServiceEntity>)roomEntity.Services).AddRange(servicesToAddEntities);

            var notFoundServicesIdsToDelete = input.ServicesToDelete
                .Where(id => !roomEntity.Services.Select((service) => service.Id).Contains(id));

            if (notFoundServicesIdsToDelete.Any())
            {
                throw new AppException(
                    new AppException.Info
                    {
                        Type = AppException.Info.TypeEnum.NotFound,
                        Code = "ServiceNotFound",
                        Message = $"Service {notFoundServicesIdsToDelete.First()} not found."
                    },
                    null
                );
            }

            var servicesToRemove = roomEntity.Services
                .Where(service => input.ServicesToDelete.Contains(service.Id))
                .ToList();

            foreach (var service in servicesToRemove)
            {
                roomEntity.Services.Remove(service);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<UpdateRoomUseCaseOutput>(roomEntity);
        }
    }
}
