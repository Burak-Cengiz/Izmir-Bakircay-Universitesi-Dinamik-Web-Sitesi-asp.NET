using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakircay.Entity.Enums
{
  public enum BakircayPages
  {
    [Display(Name = "Proje")]
    Project = 1,
    [Display(Name = "Proje")]
    Service = 2,
    [Display(Name = "Referans")]
    Reference = 3,
    [Display(Name = "Uygulamalar")]
    App = 4,
    [Display(Name = "Ürün Kategorileri")]
    ProductCategory = 5,
    [Display(Name = "Ürün")]
    Product = 6,
    Slider = 7
  }
}
