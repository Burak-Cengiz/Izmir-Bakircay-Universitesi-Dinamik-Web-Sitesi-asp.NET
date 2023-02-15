using Bakircay.Entity.Classes.ResultObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakircay.Business.BaseBusinessManagers
{
    public interface IBaseManager<T>
    {
        public Task<ResultObjectBusiness<T>> Add(T model);
        public Task<ResultObjectBusiness<T>> Update(T model);
        public Task<ResultObjectBusiness<T>> SoftDelete(string id);
        public Task<ResultObjectBusiness<T>> HardDelete(string id);
        public Task<ResultObjectBusiness<T>> GetAll();
        public Task<ResultObjectBusiness<T>> GetById(string id);
        public Task<ResultObjectBusiness<T>> ChangeStatus(string id);
    }
}
