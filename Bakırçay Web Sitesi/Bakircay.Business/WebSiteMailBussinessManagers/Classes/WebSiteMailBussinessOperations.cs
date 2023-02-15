using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.WebSiteMailBusinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.WebSiteMailClasses;
using R1Robotic.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R1Robotic.Business.WebSiteMailBusinessManagers.Classes
{
  public class WebSiteMailBussinessOperations : IWebSiteMailBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public WebSiteMailBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticWebSiteMailBusiness");
    }

    public async Task<ResultObjectBusiness<WebSiteMail>> Add(WebSiteMail model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.WebSiteMails.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<WebSiteMail>
          {
            Message = "Mail Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      }

      public async Task<ResultObjectBusiness<WebSiteMail>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentWebSiteMail = await _roboticDataContext.WebSiteMails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentWebSiteMail.IsActive = !currentWebSiteMail.IsActive;


      _roboticDataContext.Update(currentWebSiteMail);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Güncelleme Başarılı",
          ResultObject = currentWebSiteMail,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<WebSiteMail>
      {
        Message = "Mail Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentWebSiteMail,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<WebSiteMail>> GetAll()
    {
      var WebSiteMailList = await _roboticDataContext.WebSiteMails.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in WebSiteMailList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<WebSiteMail>
      {
        Message = string.Empty,
        ResultObjects = WebSiteMailList
      };
    }

    public async Task<ResultObjectBusiness<WebSiteMail>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentWebSiteMail = await _roboticDataContext.WebSiteMails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<WebSiteMail>
      {
        ResultObject = currentWebSiteMail
      };
    }

    public async Task<ResultObjectBusiness<WebSiteMail>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentWebSiteMail = await _roboticDataContext.WebSiteMails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentWebSiteMail == null)
      {
        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentWebSiteMail);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<WebSiteMail>
      {
        Message = "Mail Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<WebSiteMail>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentWebSiteMail = await _roboticDataContext.WebSiteMails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentWebSiteMail == null)
      {
        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentWebSiteMail.IsActive = false;
      currentWebSiteMail.IsDeleted = true;

      _roboticDataContext.Update(currentWebSiteMail);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<WebSiteMail>
      {
        Message = "Mail Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<WebSiteMail>> Update(WebSiteMail model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentWebSiteMail = await _roboticDataContext.WebSiteMails.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentWebSiteMail.NameSurname = model.NameSurname;
        currentWebSiteMail.Email = model.Email;
        currentWebSiteMail.PhoneNumberDefination = model.PhoneNumberDefination;
        currentWebSiteMail.Note = model.Note;


        _roboticDataContext.Update(currentWebSiteMail);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<WebSiteMail>
          {
            Message = "Mail Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<WebSiteMail>
        {
          Message = "Mail Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

    }
  }
}
