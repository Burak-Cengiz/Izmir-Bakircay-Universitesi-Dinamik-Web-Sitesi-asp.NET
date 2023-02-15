using Bakircay.Entity.Classes.ProductClasses;

namespace Bakircay.Entity.ViewModels;

public class ProductListViewModel
{
  public ProductListViewModel()
  {
    ProductFeatureList = new List<ProductFeaturesViewModel>();
    ProductAppScopeList = new List<ProductAppScope>();
    Products = new List<Product>();
    ProductCategoryList = new List<ProductCategory>();
    ProductBrandList = new List<ProductBrand>();
    ProductMountingMethList = new List<ProductMountingMeth>();
  }

  public List<ProductAppScope> ProductAppScopeList { get; set; }
  public List<ProductMountingMeth> ProductMountingMethList { get; set; }
  public List<ProductBrand> ProductBrandList { get; set; }
  public List<ProductCategory> ProductCategoryList { get; set; }
  public List<Product> Products { get; set; }
  public List<ProductFeaturesViewModel> ProductFeatureList { get; set; }
}