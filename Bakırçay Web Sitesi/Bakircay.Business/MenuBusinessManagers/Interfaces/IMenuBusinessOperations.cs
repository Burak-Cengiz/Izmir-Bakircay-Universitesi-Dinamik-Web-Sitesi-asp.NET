using Bakircay.Entity.ViewModels;

namespace Bakircay.Business.MenuBusinessManagers.Interfaces;

public interface IMenuBusinessOperations
{
  public Task<MenuViewModel> GetMenuViewModelList();
}
