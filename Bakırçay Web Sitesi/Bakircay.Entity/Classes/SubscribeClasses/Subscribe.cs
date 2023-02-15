using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.Subscribe
{
  public class Subscribe : BaseObject
  {
    [DisplayName("E-Mail")]
    [StringLength(150)]
    public string Email { get; set; }

    [NotMapped]
    public string EnchKey { get; set; }

  }
}
