using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ServiceBusinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.ServicesClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Helper.Helper;

namespace R1Robotic.Business.ServiceBusinessManagers.Classes
{
  public class ServiceOperations : IServiceOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public ServiceOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticServiceBussiness");
    }

    public async Task<ResultObjectBusiness<Service>> Add(Service model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        await _roboticDataContext.Services.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Service>
          {
            Message = "Servis Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Service>> Update(Service model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentService = await _roboticDataContext.Services.FirstOrDefaultAsync(t => t.Id == model.Id);

        if (currentService == null)
        {
          return new ResultObjectBusiness<Service>
          {
            Message = "Servis Bulunamadı",
            ResultObject = model,
            ResultStatus = ResultStatus.Info,
            Url = string.Empty
          };
        }

        currentService.ContentEn = model.ContentEn;
        currentService.ContentTr = model.ContentTr;
        currentService.DefinationTr = model.DefinationTr;


        currentService.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        currentService.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        _roboticDataContext.Update(currentService);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Service>
          {
            Message = "Servis Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Service>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Services.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentApp == null)
      {
        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentApp.IsActive = false;
      currentApp.IsDeleted = true;

      _roboticDataContext.Update(currentApp);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Service>
      {
        Message = "Servis Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Service>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Services.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentApp == null)
      {
        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentApp);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Servis Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Service>
      {
        Message = "Servis Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Service>> GetAll()
    {
      var serviceList = await _roboticDataContext.Services.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in serviceList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Service>
      {
        Message = string.Empty,
        ResultObjects = serviceList
      };
    }

    public async Task<ResultObjectBusiness<Service>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Services.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Service>
      {
        ResultObject = currentApp
      };
    }

    public async Task<ResultObjectBusiness<Service>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Services.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentApp.IsActive = !currentApp.IsActive;


      _roboticDataContext.Update(currentApp);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Service>
        {
          Message = "Durum Güncelleme Başarılı",
          ResultObject = currentApp,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Service>
      {
        Message = "Durum Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentApp,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
}
