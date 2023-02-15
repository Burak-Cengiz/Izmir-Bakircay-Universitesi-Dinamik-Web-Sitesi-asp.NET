using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.SiteSettingBussinessManager.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.SiteClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.SiteSettingBussinessManager.Classes
{
  public class SiteSettingOperations : ISiteSettingOperations
  {

    private readonly RoboticDataContext _roboticDataContext;

    public SiteSettingOperations(RoboticDataContext roboticDataContext)
    {
      _roboticDataContext = roboticDataContext;
    }


    public async Task<ResultObjectBusiness<SiteSetting>> GetById()
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProject = await _roboticDataContext.SiteSettings.FirstOrDefaultAsync();

      return new ResultObjectBusiness<SiteSetting>
      {
        ResultObject = currentProject
      };
    }


    public async Task<ResultObjectBusiness<SiteSetting>> Update(SiteSetting model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentSiteSetting = await _roboticDataContext.SiteSettings.FirstOrDefaultAsync();

        if (currentSiteSetting == null)
        {
          return new ResultObjectBusiness<SiteSetting>
          {
            Message = "Site Ayarı Bulunamadı",
            ResultObject = model,
            ResultStatus = ResultStatus.Info,
            Url = string.Empty
          };
        }

        //Site Ayarlarının Güncellendiği kısımlar
        currentSiteSetting.HomePageDescriptionTr = model.HomePageDescriptionTr;
        currentSiteSetting.HomePageDescriptionEn = model.HomePageDescriptionEn;
        currentSiteSetting.HomePageTitleTr = model.HomePageTitleTr;
        currentSiteSetting.HomePageTitleEn = model.HomePageTitleEn;
        currentSiteSetting.HomePageKeywordsEn = model.HomePageKeywordsEn;
        currentSiteSetting.HomePageKeywordsTr = model.HomePageKeywordsTr;
        currentSiteSetting.MiddleContentTr = model.MiddleContentTr;
        currentSiteSetting.MiddleContentEn = model.MiddleContentEn;
        currentSiteSetting.MiddleHeaderTr = model.MiddleHeaderTr;
        currentSiteSetting.MiddleHeaderEn = model.MiddleHeaderEn;
        currentSiteSetting.HomeCardContent1Tr = model.HomeCardContent1Tr;
        currentSiteSetting.HomeCardContent1En = model.HomeCardContent1En;
        currentSiteSetting.HomeCardHeader2Tr = model.HomeCardHeader2Tr;
        currentSiteSetting.HomeCardHeader2En = model.HomeCardHeader2En;
        currentSiteSetting.HomeCardHeader3Tr = model.HomeCardHeader3Tr;
        currentSiteSetting.HomeCardHeader3En = model.HomeCardHeader3En;
        currentSiteSetting.HomeCardConten2Tr = model.HomeCardConten2Tr;
        currentSiteSetting.HomeCardConten2En = model.HomeCardConten2En;
        currentSiteSetting.HomeCardHeader1Tr = model.HomeCardHeader1Tr;
        currentSiteSetting.HomeCardHeader1En = model.HomeCardHeader1En;
        currentSiteSetting.HomeCardContent3Tr = model.HomeCardContent3Tr;
        currentSiteSetting.HomeCardConten3En = model.HomeCardConten3En;
        currentSiteSetting.MissionHeaderTr = model.MissionHeaderTr;
        currentSiteSetting.MissionHeaderEn = model.MissionHeaderEn;
        currentSiteSetting.MissionContentTr = model.MissionContentTr;
        currentSiteSetting.MissionContentEn = model.MissionContentEn;

        currentSiteSetting.ProjectTitleTr = model.ProjectTitleTr;
        currentSiteSetting.ProjectTitleEn = model.ProjectTitleEn;
        currentSiteSetting.ProjectContentEn = model.ProjectContentEn;
        currentSiteSetting.ProjectContentTr = model.ProjectContentTr;

        currentSiteSetting.VideoSectionTitleTr = model.VideoSectionTitleTr;
        currentSiteSetting.VideoSectionTitleEn = model.VideoSectionTitleEn;
        currentSiteSetting.VideoSectionContentTr = model.VideoSectionContentTr;
        currentSiteSetting.VideoSectionContentEn = model.VideoSectionContentEn;

        currentSiteSetting.FooterSolContentTr = model.FooterSolContentTr;
        currentSiteSetting.FooterSolContentEn = model.FooterSolContentEn;
        currentSiteSetting.FooterSolYaziBasligiTr = model.FooterSolYaziBasligiTr;
        currentSiteSetting.FooterSolYaziBasligiEn = model.FooterSolYaziBasligiEn;
        currentSiteSetting.AcilYardimTelefonNo = model.AcilYardimTelefonNo;

        _roboticDataContext.Update(currentSiteSetting);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<SiteSetting>
          {
            Message = "Site Ayarları Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<SiteSetting>
        {
          Message = "Site Ayarları Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<SiteSetting>
        {
          Message = "Site Ayarları Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<SiteSetting>> Add(SiteSetting model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        _roboticDataContext.Add(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<SiteSetting>
          {
            Message = "Site Ayarları  Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<SiteSetting>
        {
          Message = "Site Ayarları  Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<SiteSetting>
        {
          Message = "Site Ayarları  Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<bool> CheckExist()
    {
      return await _roboticDataContext.SiteSettings.AnyAsync();
    }
  }
}
