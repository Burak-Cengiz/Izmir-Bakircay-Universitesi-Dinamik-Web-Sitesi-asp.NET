using Bakircay.Entity.Classes.AppClasses;
using Bakircay.Entity.Classes.ProjectClasses;
using Bakircay.Entity.Classes.ReferencesClasses;
using Bakircay.Entity.Classes.SiteClasses;
using Bakircay.Entity.Classes.SliderClasses;

namespace Bakircay.Entity.ViewModels;

public class HomePageViewModel
{
  public List<Slider> Sliders { get; set; }
  public List<App> Apps { get; set; }
  public List<Project> Projects { get; set; }
  public List<Reference> References { get; set; }
  public SiteSetting SiteSetting { get; set; }
}