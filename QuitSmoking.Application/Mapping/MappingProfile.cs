using AutoMapper;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CigarreteDto, UserCigarrete>().ReverseMap();
            CreateMap<CigarreteUpdateDto, UserCigarrete>().ReverseMap();

            CreateMap<SmokingHistoryDto, SmokingHistory>().ReverseMap();
            CreateMap<SmokingHistoryUpdateDto, SmokingHistory>().ReverseMap();
            CreateMap<SmokingHistoryGetDto, SmokingHistory>().ReverseMap();
        }
    }
}







