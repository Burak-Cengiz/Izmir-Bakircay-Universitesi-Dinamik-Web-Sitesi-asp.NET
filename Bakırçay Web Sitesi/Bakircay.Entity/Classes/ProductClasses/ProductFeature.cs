using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ProductClasses;

public class ProductFeature : BaseObject
{
  [DisplayName("Özellik")]
  public int FeatureId { get; set; }
  public Feature Feature { get; set; }

  public int ProductId { get; set; }
  public Product Product { get; set; }


  [DisplayName("Özellik Değeri")]
  public decimal Value { get; set; }

  [NotMapped]
  public string EnchKey { get; set; }
}