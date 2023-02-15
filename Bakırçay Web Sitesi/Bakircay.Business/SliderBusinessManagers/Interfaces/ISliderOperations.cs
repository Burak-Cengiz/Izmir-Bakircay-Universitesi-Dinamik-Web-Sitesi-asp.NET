using Bakircay.Business.BaseBusinessManagers;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Classes.SliderClasses;

namespace Bakircay.Business.SliderBusinessManagers.Interfaces;

public interface ISliderOperations : IBaseManager<Slider>
{
  public Task<ResultObjectBusiness<Slider>> GetActiveSliderList();
}