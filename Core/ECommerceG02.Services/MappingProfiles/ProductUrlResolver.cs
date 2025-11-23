using AutoMapper;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Shared.DTOs;
using Microsoft.Extensions.Configuration;

namespace ECommerceG02.Services.MappingProfiles
{
    public class ProductUrlResolver
        : BasePictureUrlResolver<Product, ProductDto>
    {
        public ProductUrlResolver(IConfiguration configuration) : base(configuration)
        {
        }

        public override string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            return BuildFullUrl(source.PictureUrl);
        }
    }
}
