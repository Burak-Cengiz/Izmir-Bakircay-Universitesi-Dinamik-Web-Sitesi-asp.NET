using Bakircay.Entity.Classes.BaseClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bakircay.Entity.Classes.SiteClasses;

public class SiteSetting : BaseObject
{
  #region Ana sayfa Seo
  [DisplayName("Anasayfa Seo Sayfa Başlığı (Tr)")]
  public string HomePageTitleTr { get; set; }

  [DisplayName("Anasayfa Seo Sayfa Başlığı (En)")]
  public string HomePageTitleEn { get; set; }


  [DisplayName("Anasayfa Seo Sayfa Açıklama (Tr)")]
  public string HomePageDescriptionTr { get; set; }

  [DisplayName("Anasayfa Seo Sayfa Açıklama (En)")]
  public string HomePageDescriptionEn { get; set; }


  [DisplayName("Anasayfa Seo Sayfa Keywords (Tr)")]
  public string HomePageKeywordsTr { get; set; }

  [DisplayName("Anasayfa Seo Sayfa Keywords (En)")]
  public string HomePageKeywordsEn { get; set; }
  #endregion

  #region Ana sayfa Orta İçerik

  [DisplayName("Anasayfa Orta İçerik Başlığı (Tr)")]
  public string MiddleHeaderTr { get; set; }

  [DisplayName("Anasayfa Orta İçerik Başlığı (En)")]
  public string MiddleHeaderEn { get; set; }

  [DisplayName("Anasayfa Orta İçerik  Açıklama (Tr)")]
  public string MiddleContentTr { get; set; }

  [DisplayName("Anasayfa Orta İçerik  Açıklama (En)")]
  public string MiddleContentEn { get; set; }
  #endregion

  #region Ana Sayfa Kartları
  [DisplayName("Anasayfa Kart 1 Başlık (En)")]
  public string HomeCardHeader1En { get; set; }
  [DisplayName("Anasayfa Kart 1 İçerik (En)")]
  public string HomeCardContent1En { get; set; }
  [DisplayName("Anasayfa Kart 1 Başlık (Tr)")]
  public string HomeCardHeader1Tr { get; set; }
  [DisplayName("Anasayfa Kart 1 İçerik (Tr)")]
  public string HomeCardContent1Tr { get; set; }


  [DisplayName("Anasayfa Kart 2 Başlık (En)")]
  public string HomeCardHeader2En { get; set; }
  [DisplayName("Anasayfa Kart 2 İçerik (En)")]
  public string HomeCardConten2En { get; set; }
  [DisplayName("Anasayfa Kart 2 Başlık (Tr)")]
  public string HomeCardHeader2Tr { get; set; }
  [DisplayName("Anasayfa Kart 2 İçerik (Tr)")]
  public string HomeCardConten2Tr { get; set; }



  [DisplayName("Anasayfa Kart 3 Başlık (En)")]
  public string HomeCardHeader3En { get; set; }

  [DisplayName("Anasayfa Kart 3 İçerik (En)")]
  public string HomeCardConten3En { get; set; }
  [DisplayName("Anasayfa Kart 3 Başlık (Tr)")]
  public string HomeCardHeader3Tr { get; set; }
  [DisplayName("Anasayfa Kart 3 İçerik (Tr)")]
  public string HomeCardContent3Tr { get; set; }
  #endregion

  #region Vizyon Misyon
  [DisplayName("Misyon Başlığı (Tr)")]
  public string MissionHeaderTr { get; set; }

  [DisplayName("Misyon Başlığı (En)")]
  public string MissionHeaderEn { get; set; }

  [DisplayName("Misyon İçeriği (Tr)")]
  public string MissionContentTr { get; set; }

  [DisplayName("Misyon İçeriği (En)")]
  public string MissionContentEn { get; set; }
  #endregion


  #region Proje
  [DisplayName("Proje Başlığı (Tr)")]
  public string ProjectTitleTr { get; set; }

  [DisplayName("Proje Başlığı (En)")]
  public string ProjectTitleEn { get; set; }

  [DisplayName("Proje İçeriği (Tr)")]
  public string ProjectContentTr { get; set; }

  [DisplayName("Proje İçeriği (En)")]
  public string ProjectContentEn { get; set; }
  #endregion


  #region Alt Bölüm
  [DisplayName("Video Kısım Başlığı (Tr)")]
  public string VideoSectionTitleTr { get; set; }

  [DisplayName("Video Kısım Başlığı (En)")]
  public string VideoSectionTitleEn { get; set; }

  [DisplayName("Video Kısım İçeriği (Tr)")]
  public string VideoSectionContentTr { get; set; }

  [DisplayName("Video Kısım İçeriği (En)")]
  public string VideoSectionContentEn { get; set; }
  #endregion


  #region Footer
  [DisplayName("Footer Sol Yazı Başlığı (Tr)")]
  public string FooterSolYaziBasligiTr { get; set; }

  [DisplayName("Footer Sol Yazı Başlığı (En)")]
  public string FooterSolYaziBasligiEn { get; set; }

  [DisplayName("Footer Sol Yazı İçeriği (Tr)")]
  public string FooterSolContentTr { get; set; }

  [DisplayName("Footer Sol Yazı İçeriği (En)")]
  public string FooterSolContentEn { get; set; }


  [DisplayName("Acil Yardım Telefon No")]
  public string AcilYardimTelefonNo { get; set; }
  #endregion

  [NotMapped]
  public string EnchKey { get; set; }


}