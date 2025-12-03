using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.DTO.Product;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Web.AdminHandlers.RequestHandlers;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Order;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Product;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.User;

namespace TLS.BHL.Web.AdminHandlers.AutoMapper
{
    public class WebAdminMapperProfile : Profile
    {
        public WebAdminMapperProfile()
        {
            CreateMap<UserEntity, GetListUserItem>();
            CreateMap<UpdateUserInput, UpdateUserDTO>();
            CreateMap<UserEntity, UserItem>();




            CreateMap<CreateOrderProductDTO, OrderEntity>();
            //CreateMap<OrderEntity, GetListOrderItem>()
            // .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Products))
            // .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            // .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.Created_at))
            // .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
          
        }
    }
}
