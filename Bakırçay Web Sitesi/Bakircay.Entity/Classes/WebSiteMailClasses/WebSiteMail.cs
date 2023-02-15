using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.WebSiteMailClasses
{
  public class WebSiteMail: BaseObject
  {
    [DisplayName("Kullanıcı İsmi ")]
    [StringLength(150)]
    public string NameSurname { get; set; }

    [DisplayName("E-Mail")]
    [StringLength(150)]
    public string Email { get; set; }

    [DisplayName("Telefon Numarası")]
    [StringLength(150)]
    public string PhoneNumberDefination { get; set; }

    [DisplayName("Not")]
    [StringLength(2000)]
    public string Note { get; set; }

    [NotMapped]
    public string EnchKey { get; set; }

  }
}
