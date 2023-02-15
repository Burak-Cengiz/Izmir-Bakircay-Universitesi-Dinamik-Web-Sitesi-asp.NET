using Microsoft.Extensions.DependencyInjection;
using Bakircay.Business.AppBussinessManagers.Classes;
using Bakircay.Business.AppBussinessManagers.Interfaces;
using Bakircay.Business.BlogBussinessManagers.Classes;
using Bakircay.Business.BlogBussinessManagers.Interfaces;
using Bakircay.Business.ContactBussinessManagers.Classes;  
using Bakircay.Business.ContactBussinessManagers.Interfaces;
using Bakircay.Business.CorporateBussinessManagers.Classes;
using Bakircay.Business.CorporateBussinessManagers.Interfaces;
using Bakircay.Business.ManagerUserBusinessManagers.Classes;
using Bakircay.Business.ManagerUserBusinessManagers.Interfaces;
using Bakircay.Business.MenuBusinessManagers.Classes;
using Bakircay.Business.MenuBusinessManagers.Interfaces;
using Bakircay.Business.ProductBussinessManagers.Classes;
using Bakircay.Business.ProductBussinessManagers.Interfaces;
using Bakircay.Business.ProjectBussinessManagers.Classes;
using Bakircay.Business.ProjectBussinessManagers.Interfaces;
using Bakircay.Business.ReferencesBusinessManagers.Classes;
using Bakircay.Business.ReferencesBusinessManagers.Interface;
using Bakircay.Business.SectorBussinessManagers.Classes;
using Bakircay.Business.SectorBussinessManagers.Interfaces;
using Bakircay.Business.ServiceBusinessManagers.Classes;
using Bakircay.Business.ServiceBusinessManagers.Interfaces;
using Bakircay.Business.SiteSettingBussinessManager.Classes;
using Bakircay.Business.SiteSettingBussinessManager.Interfaces;
using Bakircay.Business.SliderBusinessManagers.Classes;
using Bakircay.Business.SliderBusinessManagers.Interfaces;
using Bakircay.Business.SubscribeBusinessManagers.Classes;
using Bakircay.Business.SubscribeBusinessManagers.Interface;
using Bakircay.Business.WebSiteMailBusinessManagers.Classes;
using Bakircay.Business.WebSiteMailBusinessManagers.Interfaces;

namespace R1Robotic.Business
{
  public static class ServiceCollectionExtension
  {
    public static IServiceCollection AddRoboticManagers(this IServiceCollection service)
    {

      service.AddTransient<IAppBussinessOperations, AppBussinessOperations>();
      service.AddTransient<IManagerUserOperations, ManagerUserOperations>();
      service.AddTransient<IContactEmailBussinessOperations, ContactEmailBussinessOperations>();
      service.AddTransient<IFacilityBussinessOperations, FacilityBussinessOperations>();
      service.AddTransient<IFactoryAddressBussinessOperations, FactoryAddressBussinessOperations>();
      service.AddTransient<IPhoneNumberBussinessOperations, PhoneNumberBussinessOperations>();
      service.AddTransient<IServiceOperations, ServiceOperations>();

      service.AddTransient<ISocialMediaAccountBussinessOperations, SocialMediaAccountBussinessOperations>();
      service.AddTransient<ICorporateBussinessOperations, CorporateBussinessOperations>();
      service.AddTransient<IFeatureBussinessOperations, FeatureBussinessOperations>();
      service.AddTransient<IProductAppScopeBussinessOperation, ProductAppScopeBussinessOperation>();
      service.AddTransient<IProductBrandBussinessOperations, ProductBrandBussinessOperations>();
      service.AddTransient<IProductBussinessOperations, ProductBussinessOperations>();

      service.AddTransient<IProductCategoryBussinessOperations, ProductCategoryBussinessOperations>();
      service.AddTransient<IProductFeatureBussinessOperations, ProductFeatureBussinessOperations>();
      service.AddTransient<IProductImageBussinessOperations, ProductImageBussinessOperations>();
      service.AddTransient<IProductMountingMethBussinessOperations, ProductMountingMethBussinessOperations>();
      service.AddTransient<IProjectBussinessOperations, ProjectBussinessOperations>();
      service.AddTransient<IReferenceBussinessOperations, ReferenceBussinessOperations>();
      service.AddTransient<IBlogBussinessOperations, BlogBussinessOperations>();
      service.AddTransient<ISubscribeBussinessOperations, SubscribeBussinessOperations>();
      service.AddTransient<IWebSiteMailBussinessOperations, WebSiteMailBussinessOperations>();
      service.AddTransient<ISiteSettingOperations, SiteSettingOperations>();
      service.AddTransient<ISliderOperations, SliderOperations>();
      service.AddTransient<IMenuBusinessOperations, MenuBusinessOperations>();
      service.AddTransient<ISectorBussinessOperations, SectorBussinessOperations>();

      return service;
    }
  }
}
