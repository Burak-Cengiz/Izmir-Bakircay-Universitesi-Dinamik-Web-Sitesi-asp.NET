using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ProductClasses;

public class Feature : BaseObject
{

  [DisplayName("Özellik Tanımı Türkçe")]
  [StringLength(150)]
  public string DescriptionTr { get; set; }


  [DisplayName("Özellik Tanımı İngilizce")]
  [StringLength(150)]
  public string DescriptionEn { get; set; }

  [NotMapped]
  public string EnchKey { get; set; }
}