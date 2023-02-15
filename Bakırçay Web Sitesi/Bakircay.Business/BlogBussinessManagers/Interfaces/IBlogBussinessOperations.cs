using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.BlogClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakircay.Business.BlogBussinessManagers.Interfaces
{
  public interface IBlogBussinessOperations : IBaseManager<Blog>
  {
    public Task<ResultObjectBusiness<Blog>> GetByUrl(string id);

    public Task<ResultObjectBusiness<Blog>> GetActiveAppList();
  }
}
