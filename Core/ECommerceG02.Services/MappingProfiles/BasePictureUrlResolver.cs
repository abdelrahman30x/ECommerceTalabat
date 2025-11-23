using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace ECommerceG02.Services.MappingProfiles
{
    public abstract class BasePictureUrlResolver<TSource, TDestination>
        : IValueResolver<TSource, TDestination, string>
    {
        private readonly IConfiguration _configuration;

        protected BasePictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected string? BuildFullUrl(string? pictureUrl)
        {
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                return $"{_configuration.GetSection("Urls")["BaseUrl"]}{pictureUrl}";
            }

            return null;
        }

        public abstract string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context);
    }
}
