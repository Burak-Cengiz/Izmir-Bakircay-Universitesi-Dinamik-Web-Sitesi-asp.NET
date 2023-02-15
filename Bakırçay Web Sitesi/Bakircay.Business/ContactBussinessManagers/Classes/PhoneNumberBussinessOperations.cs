using Bakircay.Business.ContactBussinessManagers.Interfaces;
using Bakircay.Dal.Context;
using Bakircay.Entity.Classes.ContactClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Enums;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Bakircay.Business.ContactBussinessManagers.Classes
{
    public class PhoneNumberBussinessOperations : IPhoneNumberBussinessOperations
  {
    private readonly BakircayDataContext _bakircayDataContext;
    private readonly IDataProtector _dataProtector;

    public PhoneNumberBussinessOperations(BakircayDataContext bakircayDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _bakircayDataContext = bakircayDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticPhoneNumberBusiness");
    }
    public async Task<ResultObjectBusiness<PhoneNumber>> Add(PhoneNumber model)
    {

      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        model.Activity = false;
        await _bakircayDataContext.PhoneNumbers.AddAsync(model);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<PhoneNumber>
          {
            Message = "Telefon Numarası Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<PhoneNumber>> ChangeStatus(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentPhoneNumber = await _bakircayDataContext.PhoneNumbers.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentPhoneNumber.IsActive = !currentPhoneNumber.IsActive;


      _bakircayDataContext.Update(currentPhoneNumber);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Güncelleme Başarılı",
          ResultObject = currentPhoneNumber,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<PhoneNumber>
      {
        Message = "Telefon Numarası Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentPhoneNumber,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<PhoneNumber>> GetAll()
    {
      var PhoneNumberList = await _bakircayDataContext.PhoneNumbers.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in PhoneNumberList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<PhoneNumber>
      {
        Message = string.Empty,
        ResultObjects = PhoneNumberList
      };
    }

    public async Task<ResultObjectBusiness<PhoneNumber>> GetById(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentPhoneNumber = await _bakircayDataContext.PhoneNumbers.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<PhoneNumber>
      {
        ResultObject = currentPhoneNumber
      };
    }

    public async Task<ResultObjectBusiness<PhoneNumber>> HardDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentPhoneNumber = await _bakircayDataContext.PhoneNumbers.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentPhoneNumber == null)
      {
        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _bakircayDataContext.Remove(currentPhoneNumber);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<PhoneNumber>
      {
        Message = "Telefon Numarası Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<PhoneNumber>> SoftDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentPhoneNumber = await _bakircayDataContext.PhoneNumbers.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentPhoneNumber == null)
      {
        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentPhoneNumber.IsActive = false;
      currentPhoneNumber.IsDeleted = true;

      _bakircayDataContext.Update(currentPhoneNumber);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<PhoneNumber>
      {
        Message = "Telefon Numarası Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    public async Task<ResultObjectBusiness<PhoneNumber>> ChangeActivity(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentPhoneNumber = await _bakircayDataContext.PhoneNumbers.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentPhoneNumber.Activity = !currentPhoneNumber.Activity;


      _bakircayDataContext.Update(currentPhoneNumber);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Güncelleme Başarılı",
          ResultObject = currentPhoneNumber,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<PhoneNumber>
      {
        Message = "Telefon Numarası Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentPhoneNumber,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    public async Task<ResultObjectBusiness<PhoneNumber>> Update(PhoneNumber model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentPhoneNumber = await _bakircayDataContext.PhoneNumbers.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentPhoneNumber.DefinationTr = model.DefinationTr;
        currentPhoneNumber.DefinationEn = model.DefinationEn;
        currentPhoneNumber.PhoneNumberDefination = model.PhoneNumberDefination;
        currentPhoneNumber.FacilityId=model.FacilityId;



        _bakircayDataContext.Update(currentPhoneNumber);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<PhoneNumber>
          {
            Message = "Telefon Numarası Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<PhoneNumber>
        {
          Message = "Telefon Numarası Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<PhoneNumber>> GetActiveAndActivityPhoneNumber()
    {
      return new ResultObjectBusiness<PhoneNumber>()
      {
        ResultObjects = await _bakircayDataContext.PhoneNumbers.Where(t => t.IsActive && !t.IsDeleted && t.Activity).ToListAsync()
      };
    }
  }
}
