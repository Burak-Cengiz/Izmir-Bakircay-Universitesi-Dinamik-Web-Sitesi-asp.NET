using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.SliderClasses;

public class Slider : BaseObject
{
  [DisplayName("Tanım (Tr)")]
  [StringLength(150)]
  public string DefinationTr { get; set; }

  [DisplayName("Tanım (En)")]
  [StringLength(150)]
  public string DefinationEn { get; set; }

  [DisplayName("Alt Yazı (Tr)")]
  public string ContentTr { get; set; }

  [DisplayName("Alt Yazı (En)")]
  public string ContentEn { get; set; }


  [DisplayName("Button Metin (Tr)")]
  [StringLength(150)]
  public string ButtonTextTr { get; set; }

  [DisplayName("Button Metin (En)")]
  [StringLength(150)]
  public string ButtonTextEn { get; set; }

  [DisplayName("İçerik Rengi")]
  public string ContentColor { get; set; }

  [DisplayName("Button Rengi")]
  public string ButtonColor { get; set; }

  [DisplayName("Yazı Boyutu")]
  public int SliderFontSize { get; set; }

  [StringLength(300)]
  public string UrlTr { get; set; }

  [StringLength(300)]
  public string UrlEn { get; set; }

  [StringLength(300)]
  public string ImageUrlTr { get; set; }

  [StringLength(300)]
  public string ImageUrlEn { get; set; }

  [NotMapped]
  public string EnchKey { get; set; }
}