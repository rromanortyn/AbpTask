using AbpTask.Data.Entities;
using AbpTask.Modules.Reservation.Dtos.Request;
using AbpTask.Modules.Reservation.Dtos.Response;
using AbpTask.Modules.Reservation.UseCases.Inputs;
using AbpTask.Modules.Reservation.UseCases.Outputs;
using AbpTask.Modules.Room.Dtos.Request;
using AbpTask.Modules.Room.Dtos.Response;
using AbpTask.Modules.Room.UseCases.Inputs;
using AbpTask.Modules.Room.UseCases.Outputs;
using AutoMapper;

namespace AbpTask.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateRoomRequestDto, CreateRoomUseCaseInput>();
            CreateMap<CreateRoomRequestDto.Service, CreateRoomUseCaseInput.Service>();
            CreateMap<CreateRoomUseCaseOutput, RoomResponseDto>();
            CreateMap<CreateRoomUseCaseOutput.Service, RoomResponseDto.Service>();
            CreateMap<RoomEntity, UpdateRoomRequestDto>();
            CreateMap<UpdateRoomRequestDto, UpdateRoomUseCaseInput>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateRoomRequestDto.Service, UpdateRoomUseCaseInput.Service>();
            CreateMap<RoomEntity, UpdateRoomUseCaseOutput>();
            CreateMap<ServiceEntity, UpdateRoomUseCaseOutput.Service>();
            CreateMap<UpdateRoomUseCaseOutput, RoomResponseDto>();
            CreateMap<UpdateRoomUseCaseOutput.Service, RoomResponseDto.Service>();
            CreateMap<SearchRoomsRequestDto, SearchRoomsUseCaseInput>()
                .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => DateTime.Parse(src.StartAt)))
                .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => DateTime.Parse(src.EndAt)));
            CreateMap<CreateReservationRequestDto, CreateReservationUseCaseInput>()
                .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => DateTime.Parse(src.StartAt)))
                .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => DateTime.Parse(src.EndAt)));
            CreateMap<CreateReservationUseCaseOutput, CreateReservationResponseDto>();
            CreateMap<CreateReservationUseCaseOutput.Service, CreateReservationResponseDto.Service>();
            CreateMap<CreateReservationUseCaseOutput.Room, CreateReservationResponseDto.Room>();
            CreateMap<RoomEntity, SearchRoomsUseCaseOutput>();
            CreateMap<ServiceEntity, SearchRoomsUseCaseOutput.Service>();
            CreateMap<SearchRoomsUseCaseOutput, RoomResponseDto>();
            CreateMap<SearchRoomsUseCaseOutput.Service, RoomResponseDto.Service>();
        }
    }
}
