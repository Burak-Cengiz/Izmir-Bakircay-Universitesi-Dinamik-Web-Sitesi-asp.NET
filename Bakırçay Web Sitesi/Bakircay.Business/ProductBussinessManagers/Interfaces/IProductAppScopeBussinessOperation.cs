using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.ProductBussinessManagers.Interfaces;

public interface IProductAppScopeBussinessOperation : IBaseManager<ProductAppScope>
{
  public Task<ResultObjectBusiness<ProductAppScope>> GetActiveAppScopeList();
}