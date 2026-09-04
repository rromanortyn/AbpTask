using AbpTask.Modules.Room.Dtos.Request;
using AbpTask.Modules.Room.Dtos.Response;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AbpTask.Modules.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ICreateRoomUseCase _createRoomUseCase;

        public RoomsController(ICreateRoomUseCase createRoomUseCase)
        {
            _createRoomUseCase = createRoomUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<CreateRoomResponseDto>> CreateRoom([FromBody] CreateRoomRequestDto dto)
        {
            var output = await _createRoomUseCase.Execute(
                new CreateRoomUseCaseInput
                {
                    Name = dto.Name,
                    Capacity = dto.Capacity,
                    BasePricePerHour = dto.BasePricePerHour,
                    Services = [.. dto.Services.Select((service) => new CreateRoomUseCaseInput.Service
                    {
                        Name = service.Name,
                        Price = service.Price,
                    })],
                }
            );

            return CreatedAtAction(
                nameof(CreateRoom),
                new CreateRoomResponseDto
                {
                    Id = output.Id,
                    Name = output.Name,
                    Capacity = output.Capacity,
                    BasePricePerHour = output.BasePricePerHour,
                    Services = [.. output.Services.Select((service) => new CreateRoomResponseDto.Service
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Price = service.Price,
                        CreatedAt = service.CreatedAt,
                        UpdatedAt = service.UpdatedAt,
                    })],
                    CreatedAt = output.CreatedAt,
                    UpdatedAt = output.UpdatedAt,
                }
            );
        }
    }
}
