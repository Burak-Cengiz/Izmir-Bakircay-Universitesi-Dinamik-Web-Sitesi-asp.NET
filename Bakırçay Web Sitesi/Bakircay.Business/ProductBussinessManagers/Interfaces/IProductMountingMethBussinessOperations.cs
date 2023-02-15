using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.ProductBussinessManagers.Interfaces;

public interface IProductMountingMethBussinessOperations :IBaseManager<ProductMountingMeth>
{
  public Task<ResultObjectBusiness<ProductMountingMeth>> GetActiveProductMountingMethList();
}