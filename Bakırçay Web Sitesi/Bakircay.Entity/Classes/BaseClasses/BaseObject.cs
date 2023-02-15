using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.BaseClasses
{
  public class BaseObject
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime LastModifiedOn { get; set; }

    public string CreatedBy { get; set; }

    public string LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }
  }
}

