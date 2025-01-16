using AutoMapper;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CigarreteDto, Cigarretes>().ReverseMap();
            CreateMap<CigarreteUpdateDto, Cigarretes>().ReverseMap();

            CreateMap<SmokingHistoryDto, SmokingHistory>().ReverseMap();
            CreateMap<SmokingHistoryUpdateDto, SmokingHistory>().ReverseMap();
            CreateMap<SmokingHistoryGetDto, SmokingHistory>().ReverseMap();
        }
    }
}







