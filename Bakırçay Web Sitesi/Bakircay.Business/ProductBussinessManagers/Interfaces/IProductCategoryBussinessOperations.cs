using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.ProductBussinessManagers.Interfaces;

public interface IProductCategoryBussinessOperations : IBaseManager<ProductCategory>
{
  public Task<ResultObjectBusiness<ProductCategory>> List();

  public Task<ResultObjectBusiness<ProductCategory>> GetActiveProductCategoryList();
}