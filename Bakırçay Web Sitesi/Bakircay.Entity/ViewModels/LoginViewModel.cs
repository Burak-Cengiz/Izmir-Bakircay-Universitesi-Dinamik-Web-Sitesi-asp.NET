using System.ComponentModel;

namespace Bakircay.Entity.ViewModels;

public class LoginViewModel
{
  [DisplayName("E-Mail")]
  public string EMail { get; set; }
  
  [DisplayName("Parola")]
  public string Password { get; set; }
}