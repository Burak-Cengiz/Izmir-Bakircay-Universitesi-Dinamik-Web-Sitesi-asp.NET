using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Classes.SectorClasses;

namespace Bakircay.Business.SectorBussinessManagers.Interfaces;

public interface ISectorBussinessOperations: IBaseManager<Sector>
{
  public Task<ResultObjectBusiness<Sector>> GetActiveSectorList();
}