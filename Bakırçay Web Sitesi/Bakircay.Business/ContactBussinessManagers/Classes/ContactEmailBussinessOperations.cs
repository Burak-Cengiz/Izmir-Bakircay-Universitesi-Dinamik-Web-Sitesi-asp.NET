using Bakircay.Business.ContactBussinessManagers.Interfaces;
using Bakircay.Dal.Context;
using Bakircay.Entity.Classes.ContactClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Enums;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Bakircay.Business.ContactBussinessManagers.Classes
{
    public class ContactEmailBussinessOperations : IContactEmailBussinessOperations
  {
    private readonly BakircayDataContext _bakircayDataContext;
    private readonly IDataProtector _dataProtector;

    public ContactEmailBussinessOperations(BakircayDataContext bakircayDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _bakircayDataContext = bakircayDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticContactEmailBusiness");
    }
    public async Task<ResultObjectBusiness<ContactEmail>> Add(ContactEmail model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        await _bakircayDataContext.ContactEmails.AddAsync(model);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<ContactEmail>
          {
            Message = "Mail Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<ContactEmail>
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

        return new ResultObjectBusiness<ContactEmail>
        {
          Message = "Mail Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<ContactEmail>> ChangeStatus(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentContactEmail = await _bakircayDataContext.ContactEmails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentContactEmail.IsActive = !currentContactEmail.IsActive;


      _bakircayDataContext.Update(currentContactEmail);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ContactEmail>
        {
          Message = "Mail Güncelleme Başarılı",
          ResultObject = currentContactEmail,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ContactEmail>
      {
        Message = "Mail Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentContactEmail,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<ContactEmail>> GetAll()
    {
      var ContactEmailList = await _bakircayDataContext.ContactEmails.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in ContactEmailList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<ContactEmail>
      {
        Message = string.Empty,
        ResultObjects = ContactEmailList
      };
    }

    public async Task<ResultObjectBusiness<ContactEmail>> GetById(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentContactEmail = await _bakircayDataContext.ContactEmails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<ContactEmail>
      {
        ResultObject = currentContactEmail
      };
    }

    public async Task<ResultObjectBusiness<ContactEmail>> HardDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentContactEmail = await _bakircayDataContext.ContactEmails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentContactEmail == null)
      {
        return new ResultObjectBusiness<ContactEmail>
        {
          Message = "Mail Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _bakircayDataContext.Remove(currentContactEmail);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ContactEmail>
        {
          Message = "Mail Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ContactEmail>
      {
        Message = "Mail Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<ContactEmail>> SoftDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentContactEmail = await _bakircayDataContext.ContactEmails.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentContactEmail == null)
      {
        return new ResultObjectBusiness<ContactEmail>
        {
          Message = "Mail Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentContactEmail.IsActive = false;
      currentContactEmail.IsDeleted = true;

      _bakircayDataContext.Update(currentContactEmail);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ContactEmail>
        {
          Message = "Mail Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ContactEmail>
      {
        Message = "Mail Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<ContactEmail>> Update(ContactEmail model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentContactEmail = await _bakircayDataContext.ContactEmails.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentContactEmail.DefinationTr = model.DefinationTr;
        currentContactEmail.DefinationEn = model.DefinationEn;
        currentContactEmail.Emails = model.Emails;
        currentContactEmail.FacilityId = model.FacilityId;

        _bakircayDataContext.Update(currentContactEmail);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<ContactEmail>
          {
            Message = "Mail Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<ContactEmail>
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

        return new ResultObjectBusiness<ContactEmail>
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
