using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Classes.UserClasses;
using Bakircay.Entity.ViewModels;

namespace Bakircay.Business.ManagerUserBusinessManagers.Interfaces
{
  public interface IManagerUserOperations : IBaseManager<ManagerUser>
  {
    public Task<ResultObjectBusiness<ManagerUser>> GetNonDeletedUser();
    public Task<ResultObjectBusiness<ManagerUser>> GetSecuritiedUserList();
    public Task<ResultObjectBusiness<ManagerUser>> Login(LoginViewModel model);
    public Task<ResultObjectBusiness<ManagerUser>> ManagerUserFind(string id);

  }
}
