using AutoMapper;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ECommerceG02.Services.Services
{
    public class ServicesManager(
        IMapper map,
        IUnitOfWork uow,
        IBasketRepository basketRepository,
        ICacheRepository cacheRepository,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthServices> loggerAuth,
        ILogger<BasketServices> loggerBasket,
        ILogger<CacheServices> loggerCache
    ) : IServicesManager
    {
        private readonly Lazy<IProductServices> LazyProduct_services =
            new(() => new ProductServices(uow, map));
        public IProductServices ProductServices => LazyProduct_services.Value;

        private readonly Lazy<IBasketServices> LazyBasket_services =
            new(() => new BasketServices(basketRepository, map, loggerBasket));
        public IBasketServices BasketServices => LazyBasket_services.Value;

        private readonly Lazy<IAuthServices> LazyAuth_services =
            new(() => new AuthServices(userManager, signInManager, roleManager, configuration, httpContextAccessor, map, loggerAuth));
        public IAuthServices AuthenticationServices => LazyAuth_services.Value;

        private readonly Lazy<IOrderServices> LazyOrder_services =
            new(() => new OrderServices(map, basketRepository, uow));
        public IOrderServices OrderServices => LazyOrder_services.Value;

        private readonly Lazy<IPaymentServices> LazyPayment_services =
            new(() => new PaymentServices(basketRepository, uow, configuration, map));

        public IPaymentServices PaymentServices => LazyPayment_services.Value;

        private readonly Lazy<ICacheServices> LazyCache_services =
            new(() => new CacheServices(cacheRepository, loggerCache));

        public ICacheServices CacheServices => LazyCache_services.Value;

    }
}
