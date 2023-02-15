using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.ProductBussinessManagers.Interfaces;

public interface IProductBussinessOperations : IBaseManager<Product>
{
  public Task<ResultObjectBusiness<Product>> GetActiveProductList();
  public Task<ResultObjectBusiness<Product>> GetByUrl(string url);
}