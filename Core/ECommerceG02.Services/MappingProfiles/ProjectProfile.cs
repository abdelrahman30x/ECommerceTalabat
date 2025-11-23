using AutoMapper;
using ECommerceG02.Domian.Models.Baskets;
using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Domian.Models.Orders;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Presistence.Identity.Models;
using ECommerceG02.Shared.DTOs;
using ECommerceG02.Shared.DTOs.AddressDtos;
using ECommerceG02.Shared.DTOs.BasketDto_s;
using ECommerceG02.Shared.DTOs.AuthDto_s;
using ECommerceG02.Shared.DTOs.OrderDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(dist => dist.BrandName, options => options.MapFrom(src => src.Brand.Name))
            .ForMember(dist => dist.TypeName, options => options.MapFrom(src => src.Type.Name))
            .ForMember(dist => dist.PictureUrl, options => options.MapFrom<ProductUrlResolver>());

            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();

            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();

            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.IsPhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed));
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.DeliveryMethod.Price))
                 .ForMember(dest => dest.BuyerEmail, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));
            CreateMap<OrderItem, OrderItemDto>().
                ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderProductUrlResolver>());
            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
        }
    }
}
