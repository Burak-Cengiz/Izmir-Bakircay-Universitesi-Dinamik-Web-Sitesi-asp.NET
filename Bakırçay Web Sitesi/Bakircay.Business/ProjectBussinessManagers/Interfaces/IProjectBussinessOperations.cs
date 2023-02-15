using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ProjectClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.ProjectBussinessManagers.Interfaces;

public interface IProjectBussinessOperations : IBaseManager<Project>
{
  public Task<ResultObjectBusiness<Project>> GetByUrl(string url);

  public Task<ResultObjectBusiness<Project>> GetActiveProjectList();
}