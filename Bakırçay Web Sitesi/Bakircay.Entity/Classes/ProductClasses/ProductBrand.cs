using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ProductClasses;

/// <summary>
/// Ürünlerin Marka Bilgilerinin tutulduğu class
/// </summary>
public class ProductBrand : BaseObject
{
  [StringLength(150)]
  [DisplayName("Tanım")]
  public string Defination { get; set; }
  [NotMapped]
  public string EnchKey { get; set; }

}