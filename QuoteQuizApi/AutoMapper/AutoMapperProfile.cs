using AutoMapper;
using DataModels.DTOs;
using DataModels.Models;

namespace QuoteQuizApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Quote, QuoteDto>();
            CreateMap<QuoteDto, Quote>();
            CreateMap<QuoteAnswer, QuoteAnswerDto>();
            CreateMap<QuoteAnswerDto, QuoteAnswer>();
        }
    }
}
