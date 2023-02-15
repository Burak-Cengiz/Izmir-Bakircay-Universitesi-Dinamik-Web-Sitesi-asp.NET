using Bakircay.Entity.Classes.ContactClasses;

namespace Bakircay.Entity.ViewModels
{
  public class ContactPageViewModel
  {
    public List<ContactEmail> ContactEmails { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }
    public List<School> Schools { get; set; }
    public List<SchoolAddresses> SchoolAddresses { get; set; }
  }
}
