using AbpTask.Data.Entities;
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
            CreateMap<RoomEntity, UpdateRoomRequestDto>();
            CreateMap<UpdateRoomRequestDto, UpdateRoomUseCaseInput>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateRoomRequestDto.Service, UpdateRoomUseCaseInput.Service>();
            CreateMap<RoomEntity, UpdateRoomUseCaseOutput>();
            CreateMap<ServiceEntity, UpdateRoomUseCaseOutput.Service>();
            CreateMap<UpdateRoomUseCaseOutput, CreateRoomResponseDto>();
            CreateMap<UpdateRoomUseCaseOutput.Service, CreateRoomResponseDto.Service>();
        }
    }
}
