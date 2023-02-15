using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.BlogClasses
{
  public class Blog:BaseObject
  {
    [DisplayName("Tanım Türkçe")]
    [StringLength(150)]
    public string DefinationTr { get; set; }

    [DisplayName("Tanım İngilizce")]
    [StringLength(150)]
    public string DefinationEn { get; set; }

    [DisplayName("İçerik Türkçe")]
    public string ContentTr { get; set; }


    [DisplayName("İçerik İngilizce")]
    public string ContentEn { get; set; }

    [StringLength(300)]
    public string UrlTr { get; set; }

    [StringLength(300)]
    public string UrlEn { get; set; }

    [StringLength(100)]
    public string ImagePath { get; set; }

    [StringLength(100)]
    public string ShortDescTr { get; set; }

    [StringLength(100)]
    public string ShortDescEn { get; set; }

    [NotMapped]
    public string EnchKey { get; set; }
  }
}
