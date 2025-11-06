using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Order;
using Shared.DTOs.Orders;

namespace Services.Mapping.Order
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress,OrderAddressDto>().ReverseMap();
            CreateMap<Domain.Entities.Order.Order, OrderResponse>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductName, O => O.MapFrom(src => src.Product.ProductName))
                .ForMember(D => D.ProductId, O => O.MapFrom(src => src.Product.ProductId))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(src => src.Product.PictureUrl));

            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
        }
    }
}
