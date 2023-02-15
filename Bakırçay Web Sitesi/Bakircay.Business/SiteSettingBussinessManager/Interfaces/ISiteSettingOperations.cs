using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Classes.SiteClasses;

namespace Bakircay.Business.SiteSettingBussinessManager.Interfaces
{
  public interface ISiteSettingOperations
  {
    public Task<ResultObjectBusiness<SiteSetting>> GetById();
    public Task<ResultObjectBusiness<SiteSetting>> Update(SiteSetting model);
    public Task<ResultObjectBusiness<SiteSetting>> Add(SiteSetting model);
    public Task<bool> CheckExist();
  }
}
