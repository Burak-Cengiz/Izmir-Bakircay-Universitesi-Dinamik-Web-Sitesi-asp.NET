using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ContactClasses
{
  public class SchoolAddresses : BaseObject
  {
    [DisplayName("Tanım Türkçe")]
    [StringLength(150)]
    public string DefinationTr { get; set; }

    [DisplayName("Tanım İngilizce")]
    [StringLength(150)]
    public string DefinationEn { get; set; }

    [DisplayName("Adres Türkçe")]
    [StringLength(150)]
    public string AdressTr { get; set; }

    [DisplayName("Adres İngilizce")]
    [StringLength(150)]
    public string AdressEn { get; set; }

    [DisplayName("Okul")]
    public int SchoolId { get; set; }
    public School School { get; set; }

    [DisplayName("Konum")]
    public string Location { get; set; }
    [NotMapped]
    public string EnchKey { get; set; }

  }
}
