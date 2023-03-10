using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ContactClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakircay.Business.ContactBussinessManagers.Interfaces
{
  public interface IPhoneNumberBussinessOperations : IBaseManager<PhoneNumber>
  {
    public Task<ResultObjectBusiness<PhoneNumber>> ChangeActivity(string id);
    public Task<ResultObjectBusiness<PhoneNumber>> GetActiveAndActivityPhoneNumber();
  }
}
