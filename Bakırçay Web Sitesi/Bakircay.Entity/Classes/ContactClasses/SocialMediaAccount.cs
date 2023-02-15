using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ContactClasses
{
  public class SocialMediaAccount:BaseObject
  {
    [DisplayName("Tanım Türkçe")]
    [StringLength(150)]
    public string DefinationTr { get; set; }

    [DisplayName("Tanım İngilizce")]
    [StringLength(150)]
    public string DefinationEn { get; set; }

    [DisplayName("Instagram Hesabı")]
    [StringLength(150)]
    public string Instagram { get; set; }

    [DisplayName("Youtube Hesabı")]
    [StringLength(150)]
    public string Youtube { get; set; }

    [DisplayName("Facebook Hesabı")]
    [StringLength(150)]
    public string Facebook { get; set; }

    [DisplayName("Linkedin Hesabı")]
    [StringLength(150)]
    public string Twitter { get; set; }
    [NotMapped]
    public string EnchKey { get; set; }


  }
}
