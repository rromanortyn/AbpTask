using AbpTask.Modules.Room.Dtos.Request;
using AbpTask.Modules.Room.Dtos.Response;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbpTask.Modules.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ICreateRoomUseCase _createRoomUseCase;

        public RoomController(ICreateRoomUseCase createRoomUseCase)
        {
            _createRoomUseCase = createRoomUseCase;
        }

        [HttpPost]
        public ActionResult<CreateRoomResponseDto> CreateRoom([FromBody] CreateRoomRequestDto dto)
        {
            var output = _createRoomUseCase.Execute(
                new CreateRoomUseCaseInput
                {
                    Name = dto.Name,
                    Capacity = dto.Capacity,
                    BasePricePerHour = dto.BasePricePerHour,
                }
            );

            return CreatedAtAction(nameof(CreateRoom), new { output.Id });
        }
    }
}
