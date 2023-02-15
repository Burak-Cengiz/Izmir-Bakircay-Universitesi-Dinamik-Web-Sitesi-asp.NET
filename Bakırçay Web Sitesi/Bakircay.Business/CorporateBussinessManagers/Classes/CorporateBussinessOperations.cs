using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.CorporateBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.CorporateClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Helper.Helper;

namespace R1Robotic.Business.CorporateBussinessManagers.Classes
{
  public class CorporateBussinessOperations : ICorporateBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public CorporateBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticCorporateBusiness");
    }

    public async Task<ResultObjectBusiness<Corporate>> Add(Corporate model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);
        await _roboticDataContext.Corporations.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Corporate>
          {
            Message = "Kurumsal Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Corporate>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentCorporate = await _roboticDataContext.Corporations.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentCorporate.IsActive = !currentCorporate.IsActive;


      _roboticDataContext.Update(currentCorporate);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Güncelleme Başarılı",
          ResultObject = currentCorporate,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Corporate>
      {
        Message = "Kurumsal Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentCorporate,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Corporate>> GetActiveCorporateList()
    {
      var currentApps =
        await _roboticDataContext.Corporations.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

      return new ResultObjectBusiness<Corporate>
      {
        ResultObjects = currentApps
      };
    }

    public async Task<ResultObjectBusiness<Corporate>> GetByUrl(string url)
    {
      var currentApp =
        await _roboticDataContext.Corporations.FirstOrDefaultAsync(t =>
          t.UrlTr == url || t.UrlEn == url && !t.IsDeleted && t.IsActive);

      return new ResultObjectBusiness<Corporate>
      {
        ResultObject = currentApp
      };
    }

    public async Task<ResultObjectBusiness<Corporate>> GetAll()
    {
      var appList = await _roboticDataContext.Corporations.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in appList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Corporate>
      {
        Message = string.Empty,
        ResultObjects = appList
      };

    }

    public async Task<ResultObjectBusiness<Corporate>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentCorporate = await _roboticDataContext.Corporations.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Corporate>
      {
        ResultObject = currentCorporate
      };

    }

    public async Task<ResultObjectBusiness<Corporate>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentCorporate = await _roboticDataContext.Corporations.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentCorporate == null)
      {
        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentCorporate);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Corporate>
      {
        Message = "Kurumsal Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Corporate>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentCorporate = await _roboticDataContext.Corporations.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentCorporate == null)
      {
        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentCorporate.IsActive = false;
      currentCorporate.IsDeleted = true;

      _roboticDataContext.Update(currentCorporate);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Corporate>
      {
        Message = "Kurumsal Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Corporate>> Update(Corporate model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentCorporate = await _roboticDataContext.Corporations.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentCorporate.ContentEn = model.ContentEn;
        currentCorporate.ContentTr = model.ContentTr;
        currentCorporate.DefinationTr = model.DefinationTr;
        currentCorporate.DefinationEn = model.DefinationEn;
        currentCorporate.ShortDescTr = model.ShortDescTr;
        currentCorporate.ShortDescEn = model.ShortDescEn;

        currentCorporate.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        currentCorporate.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        _roboticDataContext.Update(currentCorporate);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Corporate>
          {
            Message = "Kurumsal Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Kurumsal Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Corporate>
        {
          Message = "Uygulama Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }
}
