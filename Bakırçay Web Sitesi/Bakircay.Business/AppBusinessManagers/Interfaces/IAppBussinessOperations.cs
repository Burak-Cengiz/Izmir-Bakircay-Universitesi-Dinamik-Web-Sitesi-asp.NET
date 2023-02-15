using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.AppClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;

namespace Bakircay.Business.AppBusinessManagers.Interfaces
{
    public interface IAppBussinessOperations : IBaseManager<App>
    {
        public Task<ResultObjectBusiness<App>> AddresGetir();
    }
}
