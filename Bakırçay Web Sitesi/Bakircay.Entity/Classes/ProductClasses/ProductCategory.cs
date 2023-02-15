using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ProductClasses
{
  public class ProductCategory : BaseObject
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


    [DisplayName("Bağlı Olduğu Kategori")]
    public int? ParentCategoryId { get; set; }
    public ProductCategory ParentCategory { get; set; }

    [DisplayName("Kategori Sayfa Başlığı")]
    public string PageTitle { get; set; }


    [DisplayName("Kategori Sayfa Açıklaması")]
    public string PageDescription { get; set; }


    [DisplayName("Kategori Sayfa Anahtar Kelimeleri")]
    public string PageKeywords { get; set; }

    public ICollection<Product> Products { get; set; }

    [NotMapped]
    public string EnchKey { get; set; }


  }
}
