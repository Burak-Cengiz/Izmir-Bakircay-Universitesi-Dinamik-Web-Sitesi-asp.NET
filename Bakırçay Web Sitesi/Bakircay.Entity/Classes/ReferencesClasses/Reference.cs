using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ReferencesClasses
{
  public class Reference : BaseObject
  {
    [DisplayName("Tanım")]
    [StringLength(150)]
    public string Defination { get; set; }


    [DisplayName("Dosya")]
    public string ImageFileName { get; set; }

    [NotMapped]
    public string EnchKey { get; set; }

  }
}
