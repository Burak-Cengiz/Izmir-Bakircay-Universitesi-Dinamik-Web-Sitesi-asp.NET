using Bakircay.Entity.Classes.CorporateClasses;
using Bakircay.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ServicesClasses;

namespace Bakircay.Entity.ViewModels;

public class MenuViewModel
{
  public List<Corporate> Corporates { get; set; }
  public List<Service> Services { get; set; }
  public List<ProductCategory> ProductCategories { get; set; }
}