using AbpTask.Modules.Room.Dtos.Request;
using AbpTask.Modules.Room.Dtos.Response;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Interfaces;
using AbpTask.Shared;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AbpTask.Modules.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreateRoomUseCase _createRoomUseCase;
        private readonly IUpdateRoomUseCase _updateRoomUseCase;
        private readonly IDeleteRoomUseCase _deleteRoomUseCase;
        private readonly ISearchRoomsUseCase _searchRoomsUseCase;


        public RoomsController(
            IMapper mapper,
            ICreateRoomUseCase createRoomUseCase,
            IUpdateRoomUseCase updateRoomUseCase,
            IDeleteRoomUseCase deleteRoomUseCase,
            ISearchRoomsUseCase searchRoomsUseCase
        )
        {
            _mapper = mapper;
            _createRoomUseCase = createRoomUseCase;
            _updateRoomUseCase = updateRoomUseCase;
            _deleteRoomUseCase = deleteRoomUseCase;
            _searchRoomsUseCase = searchRoomsUseCase;
        }

        [HttpPost]
        [ProducesResponseType<RoomResponseDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomResponseDto>> CreateRoom([FromBody] CreateRoomRequestDto dto)
        {
            var useCaseInput = _mapper.Map<CreateRoomUseCaseInput>(dto);

            var useCaseOutput = await _createRoomUseCase.Execute(useCaseInput);

            var responseDto = _mapper.Map<RoomResponseDto>(useCaseOutput);

            return CreatedAtAction(nameof(CreateRoom), responseDto);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType<RoomResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomResponseDto>> UpdateRoom(
            [FromRoute] long id,
            [FromBody] JsonPatchDocument<UpdateRoomRequestDto> patchDoc
        )
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var dtoToPatch = new UpdateRoomRequestDto();
            var useCaseInput = new UpdateRoomUseCaseInput { Id = id };

            var servicesOperations = patchDoc.Operations
                .Where(op => op.path.StartsWith("/services", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var operation in servicesOperations)
            {
                if (operation.value is JsonElement jsonElement)
                {
                    if (operation.OperationType == Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.OperationType.Add)
                    {
                        var servicesToAdd = JsonSerializer.Deserialize<List<UpdateRoomRequestDto.Service>>(
                            jsonElement.GetRawText(),
                            JsonSerializerOptions.Web
                        );

                        if (servicesToAdd != null)
                        {
                            dtoToPatch.ServicesToAdd = servicesToAdd;
                        }

                        patchDoc.Operations.Remove(operation);
                    }

                    else if (operation.OperationType == Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.OperationType.Remove)
                    {
                        var servicesToDelete = JsonSerializer.Deserialize<List<long>>(
                            jsonElement.GetRawText(),
                            JsonSerializerOptions.Web
                        );

                        if (servicesToDelete != null)
                        {
                            dtoToPatch.ServicesToDelete = servicesToDelete;
                        }

                        patchDoc.Operations.Remove(operation);
                    }
                }
            }

            patchDoc.ApplyTo(dtoToPatch, error =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? string.Empty, error.ErrorMessage);
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(dtoToPatch))
            {
                return BadRequest(ModelState);
            }

            foreach (var operation in patchDoc.Operations)
            {
                if (operation.value is JsonElement jsonElement)
                {
                    if (operation.OperationType == Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations.OperationType.Replace)
                    {
                        if (operation.path.Equals("/name", StringComparison.OrdinalIgnoreCase))
                        {
                            useCaseInput.Name = jsonElement.GetString();
                        }

                        else if (operation.path.Equals("/capacity", StringComparison.OrdinalIgnoreCase))
                        {
                            useCaseInput.Capacity = jsonElement.GetInt32();
                        }

                        else if (operation.path.Equals("/basePricePerHour", StringComparison.OrdinalIgnoreCase))
                        {
                            useCaseInput.BasePricePerHour = jsonElement.GetDouble();
                        }
                    }
                }
            }

            if (dtoToPatch != null && dtoToPatch.ServicesToAdd != null)
            {
                useCaseInput.ServicesToAdd = _mapper.Map<List<UpdateRoomUseCaseInput.Service>>(dtoToPatch.ServicesToAdd);

            }

            if (dtoToPatch != null && dtoToPatch.ServicesToDelete != null)
            {
                useCaseInput.ServicesToDelete = dtoToPatch.ServicesToDelete;
            }

            var useCaseOutput = await _updateRoomUseCase.Execute(useCaseInput);
            var responseDto = _mapper.Map<RoomResponseDto>(useCaseOutput);

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRoom([FromRoute] long id)
        {
            var useCaseInput = new DeleteRoomUseCaseInput { Id = id };

            await _deleteRoomUseCase.Execute(useCaseInput);

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType<List<RoomResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RoomResponseDto>>> SearchRooms([FromQuery] SearchRoomsRequestDto dto)
        {
            var useCaseInput = _mapper.Map<SearchRoomsUseCaseInput>(dto);

            var useCaseOutput = await _searchRoomsUseCase.Execute(useCaseInput);

            var responseDto = _mapper.Map<List<RoomResponseDto>>(useCaseOutput);

            return Ok(responseDto);
        }
    }
}
