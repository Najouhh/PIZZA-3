using AutoMapper;
using Pizza.Data;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.DTOS.Order;
using Pizza.Data.Models.DTOS.User;
using Pizza.Data.Models.Entities;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateMap<DishDto, Dish>()
            // .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { CategoryID = src.CategoryId }))
            //.ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src..Select(id => new Ingredient { IngredientID = id }).ToList()));
            CreateMap<DishDto, Dish>()
      .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { CategoryID = src.CategoryID }))
      .ForMember(dest => dest.Ingredients, opt => opt.Ignore());


            CreateMap<Dish, DishDto>();
            CreateMap<DishDto, Dish>();


            // for users
            CreateMap<UserRegister, ApplicationUser>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
             .ForMember(dest => dest.UserPoints, opt => opt.MapFrom(src => 0));
            // for another dto
            CreateMap<ApplicationUser, UserGet>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.UserPoints, opt => opt.MapFrom(src => src.UserPoints))
            .ForMember(dest => dest.Role, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Role"]));
            CreateMap<ApplicationUser, UserUpdate>();
            CreateMap<UserUpdate, ApplicationUser>();
            CreateMap<ApplicationUser, UserLogin>();
            CreateMap<UserLogin, ApplicationUser>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
        }
    }
}
