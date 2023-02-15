using Bakircay.Business.AppBusinessManagers.Interfaces;
using Bakircay.Dal.Context;
using Bakircay.Entity.Classes.AppClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Enums;
using Bakircay.Helper.Helper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Bakircay.Business.AppBusinessManagers.Classes
{
    public class AppBussinessOperations : IAppBussinessOperations
  {
    private readonly BakircayDataContext _bakircayDataContext;
    private readonly IDataProtector _dataProtector;

    public AppBussinessOperations(BakircayDataContext bakircayDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _bakircayDataContext = bakircayDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticAppBussiness");
    }

    public async Task<ResultObjectBusiness<App>> Add(App model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);
        await _bakircayDataContext.Apps.AddAsync(model);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<App>
          {
            Message = "Uygulama Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<App>> Update(App model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentApp = await _bakircayDataContext.Apps.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentApp.ContentEn = model.ContentEn;
        currentApp.ContentTr = model.ContentTr;
        currentApp.DefinationTr = model.DefinationTr;
        currentApp.ImagePathTr = string.IsNullOrEmpty(model.ImagePathTr) ? currentApp.ImagePathTr = currentApp.ImagePathTr : currentApp.ImagePathTr = model.ImagePathTr;

        currentApp.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        currentApp.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        _bakircayDataContext.Update(currentApp);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<App>
          {
            Message = "Uygulama Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<App>> SoftDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentApp = await _bakircayDataContext.Apps.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentApp == null)
      {
        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentApp.IsActive = false;
      currentApp.IsDeleted = true;

      _bakircayDataContext.Update(currentApp);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<App>
      {
        Message = "Uygulama Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<App>> HardDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentApp = await _bakircayDataContext.Apps.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentApp == null)
      {
        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _bakircayDataContext.Remove(currentApp);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Uygulama Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<App>
      {
        Message = "Uygulama Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<App>> GetAll()
    {
      var appList = await _bakircayDataContext.Apps.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in appList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<App>
      {
        Message = string.Empty,
        ResultObjects = appList
      };

    }

    public async Task<ResultObjectBusiness<App>> GetById(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentApp = await _bakircayDataContext.Apps.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<App>
      {
        ResultObject = currentApp
      };

    }

    public async Task<ResultObjectBusiness<App>> ChangeStatus(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentApp = await _bakircayDataContext.Apps.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentApp.IsActive = !currentApp.IsActive;


      _bakircayDataContext.Update(currentApp);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<App>
        {
          Message = "Durum Güncelleme Başarılı",
          ResultObject = currentApp,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<App>
      {
        Message = "Durum Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentApp,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public Task<ResultObjectBusiness<App>> AddresGetir()
    {
      return null;
    }
  }
}
