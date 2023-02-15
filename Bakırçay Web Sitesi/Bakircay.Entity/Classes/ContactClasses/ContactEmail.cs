using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ContactClasses
{
  public class ContactEmail : BaseObject
  {
    [DisplayName("Tanım Türkçe")]
    [StringLength(150)]
    public string DefinationTr { get; set; }

    [DisplayName("Tanım İngilizce")]
    [StringLength(150)]
    public string DefinationEn { get; set; }

    [DisplayName("E-Mailler")]
    [StringLength(150)]
    public string Emails { get; set; }

    [DisplayName("Tesis")]
    public int FacilityId { get; set; }
    public SchoolAddresses Facility { get; set; }

    [NotMapped]
    public string EnchKey { get; set; }
  }
}
