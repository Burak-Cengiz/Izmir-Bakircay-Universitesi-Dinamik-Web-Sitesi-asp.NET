using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bakircay.Entity.Classes.BlogClasses;

public class BlogImage : BaseObject
{
  public int BlogId { get; set; }
  public Blog Blog { get; set; }


  [DisplayName("Resim Yolu")]
  [StringLength(150)]
  public string ImagePath { get; set; }
}