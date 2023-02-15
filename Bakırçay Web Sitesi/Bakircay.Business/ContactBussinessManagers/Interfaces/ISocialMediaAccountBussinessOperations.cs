using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ContactClasses;

namespace Bakircay.Business.ContactBussinessManagers.Interfaces;

public interface ISocialMediaAccountBussinessOperations : IBaseManager<SocialMediaAccount>
{
  public SocialMediaAccount GetSocialMediaAccount();
  public Task<bool> CheckSocialMediaExist();
}