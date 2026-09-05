using AbpTask.Data;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AbpTask.Modules.Room.UseCases.Implementations
{
    public class DeleteRoomUseCase : IDeleteRoomUseCase
    {
        private readonly ApplicationDbContext _context;

        public DeleteRoomUseCase(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Execute(DeleteRoomUseCaseInput input)
        {
            var roomById = await _context.Rooms
                .FirstOrDefaultAsync((room) => room.Id == input.Id);

            if (roomById == null)
            {
                throw new Exception("Room not found");
            }

            _context.Rooms.Remove(roomById);

            await _context.SaveChangesAsync();

            return Task.FromResult<object>(null);
        }
    }
}
