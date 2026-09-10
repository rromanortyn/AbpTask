using AbpTask.Modules.Reservation.Dtos.Request;
using AbpTask.Modules.Reservation.Dtos.Response;
using AbpTask.Modules.Reservation.UseCases.Inputs;
using AbpTask.Modules.Reservation.UseCases.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AbpTask.Modules.Reservation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreateReservationUseCase _createReservationUseCase;

        public ReservationsController(IMapper mapper, ICreateReservationUseCase createReservationUseCase)
        {
            _mapper = mapper;
            _createReservationUseCase = createReservationUseCase;
        }

        [HttpPost]
        public async Task<ActionResult> CreateReservation([FromBody] CreateReservationRequestDto dto)
        {
            var useCaseInput = _mapper.Map<CreateReservationUseCaseInput>(dto);
            
            var useCaseOutput = await _createReservationUseCase.Execute(useCaseInput);
            
            var responseDto = _mapper.Map<CreateReservationResponseDto>(useCaseOutput);

            return CreatedAtAction(nameof(CreateReservation), responseDto);
        }
    }
}
