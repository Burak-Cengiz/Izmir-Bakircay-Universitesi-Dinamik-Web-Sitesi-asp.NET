using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.UserClasses
{
  public class ManagerUser : BaseObject
  {

    [StringLength(150)]
    [DisplayName("Ad")]
    public string Name { get; set; }


    [StringLength(150)]
    [DisplayName("Soyad")]
    public string SurName { get; set; }

    [DisplayName("Ad Soyad")]
    public string FullName => $"{Name} {SurName}";


    [StringLength(100)]
    [DisplayName("Parola")]
    public string Password { get; set; }

    [NotMapped]
    [DisplayName("Parola Tekrar")]
    public string PasswordRepeat { get; set; }

    [StringLength(256)]
    [DisplayName("E-Mail")]
    public string EMail { get; set; }


    [NotMapped]
    public string ManagerUserEncrypedId { get; set; }
  }
}
