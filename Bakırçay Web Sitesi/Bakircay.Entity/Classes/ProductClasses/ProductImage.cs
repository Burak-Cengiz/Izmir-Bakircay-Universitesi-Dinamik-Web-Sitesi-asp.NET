using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ProductClasses
{
  public class ProductImage : BaseObject
  {
    public int ProductId { get; set; }
    public Product Product { get; set; }


    [DisplayName("Özellik Değeri")]
    [StringLength(100)]
    public string ImagePath { get; set; }

    [NotMapped]
  public string EnchKey { get; set; }
  }
}
