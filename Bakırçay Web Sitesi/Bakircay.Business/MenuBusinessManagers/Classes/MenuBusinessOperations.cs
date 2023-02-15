using R1Robotic.Business.CorporateBussinessManagers.Interfaces;
using R1Robotic.Business.MenuBusinessManagers.Interfaces;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Business.ServiceBusinessManagers.Interfaces;
using R1Robotic.Entity.ViewModels;

namespace R1Robotic.Business.MenuBusinessManagers.Classes;

public class MenuBusinessOperations : IMenuBusinessOperations
{
  private readonly ICorporateBussinessOperations _corporateBussinessOperations;
  private readonly IServiceOperations _serviceOperations;
  private readonly IProductCategoryBussinessOperations _productCategoryBussinessOperations;

  public MenuBusinessOperations(ICorporateBussinessOperations corporateBussinessOperations, IServiceOperations serviceOperations, IProductCategoryBussinessOperations productCategoryBussinessOperations)
  {
    _corporateBussinessOperations = corporateBussinessOperations;
    _serviceOperations = serviceOperations;
    _productCategoryBussinessOperations = productCategoryBussinessOperations;
  }

  public async Task<MenuViewModel> GetMenuViewModelList()
  {
    MenuViewModel lstMenuViewModels = new MenuViewModel();

    var corporateList = await _corporateBussinessOperations.GetActiveCorporateList();
    var serviceList = await _serviceOperations.GetActiveServiceList();
    var productCategories = await _productCategoryBussinessOperations.List();

    var cleanProductCategoryList = productCategories.ResultObjects.Where(t => t.IsActive).ToList();

    lstMenuViewModels.Corporates = corporateList.ResultObjects.ToList();
    lstMenuViewModels.Services = serviceList.ResultObjects.ToList();
    lstMenuViewModels.ProductCategories = cleanProductCategoryList;


    return lstMenuViewModels;
  }
}