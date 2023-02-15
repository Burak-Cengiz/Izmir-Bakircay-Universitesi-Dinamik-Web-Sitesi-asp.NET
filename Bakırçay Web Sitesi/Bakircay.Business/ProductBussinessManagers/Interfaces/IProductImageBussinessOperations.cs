using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.ProductBussinessManagers.Interfaces;

public interface IProductImageBussinessOperations : IBaseManager<ProductImage>
{
  public Task<ResultObjectBusiness<ProductImage>> GetProductImageList(int productId);

  public Task<bool> Delete(int id);
}