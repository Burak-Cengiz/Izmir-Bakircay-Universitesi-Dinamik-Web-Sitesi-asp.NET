using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.ProductClasses;

public class Product : BaseObject
{
  [DisplayName("Ürün İsmi Türkçe")]
  [StringLength(150)]
  public string NameTr { get; set; }


  [DisplayName("Ürün İsmi İngilizce")]
  [StringLength(150)]
  public string NameEn { get; set; }

  [DisplayName("Özellik  Türkçe")]
  [StringLength(150)]
  public string TechnicDetailsTr { get; set; }

  [DisplayName("Özellik  İngilizce")]
  [StringLength(150)]
  public string TechnicDetailsEn { get; set; }

  [DisplayName("İçerik  Türkçe")]
  [StringLength(150)]
  public string ContentTr { get; set; }

  [DisplayName("İçerik  İngilizce")]
  [StringLength(150)]
  public string ContentEn { get; set; }

  [StringLength(300)]
  public string UrlTr { get; set; }

  [StringLength(300)]
  public string UrlEn { get; set; }


  [DisplayName("Ürün Sayfa Başlığı (Tr)")]
  public string PageTitleTr { get; set; }

  [DisplayName("Ürün Sayfa Başlığı (En)")]
  public string PageTitleEn { get; set; }


  [DisplayName("Ürün Sayfa Açıklaması (Tr)")]
  public string PageDescriptionTr { get; set; }

  [DisplayName("Ürün Sayfa Açıklaması (En)")]
  public string PageDescriptionEn { get; set; }


  [DisplayName("Ürün Sayfa Anahtar Kelimeleri (Tr)")]
  public string PageKeywordsTr { get; set; }

  [DisplayName("Ürün Sayfa Anahtar Kelimeleri (En)")]
  public string PageKeywordsEn { get; set; }


  [DisplayName("Ürün Kategorisi")]
  public int ProductCategoryId { get; set; }
  public ProductCategory ProductCategory { get; set; }

  [DisplayName("Ürün Markası")]
  public int ProductBrandId { get; set; }
  public ProductBrand ProductBrand { get; set; }


  [DisplayName("Ürün Montaj Şekli")]
  public int ProductMountingMethId { get; set; }
  public ProductMountingMeth ProductMountingMeth { get; set; }//ürün yapım methodu olarak bir değişkenimiz var bunu duruma göre kaldırmalıyız.


  [DisplayName("Ürün Uygulama Alanı")]
  public int ProductAppScopeId { get; set; }
  public ProductAppScope ProductAppScope { get; set; }//Aynı şekilde kaldırılabilir.


  public int ViewCount { get; set; }


  public ICollection<ProductFeature> ProductFeatures { get; set; }

  public ICollection<ProductImage> ProductImages { get; set; }


  [NotMapped]
  public string EnchKey { get; set; }
}