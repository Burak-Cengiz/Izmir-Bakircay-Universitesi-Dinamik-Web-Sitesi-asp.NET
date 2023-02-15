using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.CorporateClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.CorporateBussinessManagers.Interfaces;

public interface ICorporateBussinessOperations : IBaseManager<Corporate>
{
  public Task<ResultObjectBusiness<Corporate>> GetActiveCorporateList();
  public Task<ResultObjectBusiness<Corporate>> GetByUrl(string url);
}