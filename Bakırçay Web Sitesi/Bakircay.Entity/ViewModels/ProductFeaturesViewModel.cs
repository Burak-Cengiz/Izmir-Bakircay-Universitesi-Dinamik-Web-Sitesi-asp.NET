using Bakircay.Entity.Classes.ProductClasses;

namespace Bakircay.Entity.ViewModels;

public class ProductFeaturesViewModel
{
  public int FeatureId { get; set; }
  public Feature Feature { get; set; }

  public decimal MinValue { get; set; }

  public decimal MaxValue { get; set; }
}