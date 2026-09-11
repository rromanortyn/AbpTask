using AbpTask.Data;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Modules.Room.UseCases.Outputs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class SearchRoomsUseCase : ISearchRoomsUseCase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchRoomsUseCase(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SearchRoomsUseCaseOutput>> Execute(SearchRoomsUseCaseInput input)
        {
            var existingReservations = await _context.Reservations
                .Where(reservation => reservation.StartAt <= input.EndAt.ToUniversalTime() && reservation.EndAt >= input.StartAt.ToUniversalTime())
                .ToListAsync();

            var reservedRoomsIds = existingReservations
                .Select(reservation => reservation.RoomId)
                .ToList();

            // if the room has 3 services, returns 3 records from DB instead of 1
            // TODO: make separate queries
            var availableRooms = await _context.Rooms
                .Include(room => room.Services)
                .Where(room => !reservedRoomsIds.Contains(room.Id) && room.Capacity >= input.Capacity)
                .ToListAsync();

            return _mapper.Map<List<SearchRoomsUseCaseOutput>>(availableRooms);
        }
    }
}
