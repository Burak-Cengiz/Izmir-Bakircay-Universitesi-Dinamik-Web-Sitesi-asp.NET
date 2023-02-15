using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.ViewModels;

namespace Bakircay.Business.ProductBussinessManagers.Interfaces;

public interface IProductFeatureBussinessOperations : IBaseManager<ProductFeature>
{
  public Task<bool> CheckExistProductFeature(ProductFeature model);

  public Task<bool> CheckExistProductFeatureUpdate(ProductFeature model);

  public Task<ResultObjectBusiness<ProductFeature>> GetProductFeatureById(int id);

  public Task<ResultObjectBusiness<ProductFeature>> GetProductFeaturesNonDeletedAndActiveProductId(int id);

  public Task<ResultObjectBusiness<ProductFeature>> Delete(int id);

  public Task<ResultObjectBusiness<ProductFeature>> ChangeBaseStatus(int id);

  public Task<List<ProductFeaturesViewModel>> GetFeaturedForFilter();

  public Task<List<Product>> GetProductList(ProductFeaturesViewModel model);

}