using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ContactClasses;

public class School : BaseObject
{
  [DisplayName("Tanım Türkçe")]
  [StringLength(150)]
  public string DefinationTr { get; set; }


  [DisplayName("Tanım İngilizce")]
  [StringLength(150)]
  public string DefinationEn { get; set; }
  [NotMapped]
  public string EnchKey { get; set; }

  public ICollection<SchoolAddresses> SchoolAddresses { get; set; }
  public ICollection<PhoneNumber> PhoneNumbers { get; set; }
  public ICollection<ContactEmail> ContactEmails { get; set; }
}