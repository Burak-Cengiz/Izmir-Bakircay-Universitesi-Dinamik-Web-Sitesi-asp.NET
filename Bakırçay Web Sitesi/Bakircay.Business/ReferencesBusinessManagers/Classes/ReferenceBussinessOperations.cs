using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ReferencesBusinessManagers.Interface;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ReferencesClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
namespace R1Robotic.Business.ReferencesBusinessManagers.Classes
{
  public class ReferenceBussinessOperations : IReferenceBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public ReferenceBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticReferenceBussiness");
    }


    public async Task<ResultObjectBusiness<Reference>> Add(Reference model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.References.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Reference>
          {
            Message = "Referans Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Reference>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentReference = await _roboticDataContext.References.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentReference.IsActive = !currentReference.IsActive;


      _roboticDataContext.Update(currentReference);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Güncelleme Başarılı",
          ResultObject = currentReference,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Reference>
      {
        Message = "Referans Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentReference,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Reference>> GetAll()
    {
      var ReferenceList = await _roboticDataContext.References.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in ReferenceList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Reference>
      {
        Message = string.Empty,
        ResultObjects = ReferenceList
      };
    }

    public async Task<ResultObjectBusiness<Reference>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentReference = await _roboticDataContext.References.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Reference>
      {
        ResultObject = currentReference
      };
    }

    public async Task<ResultObjectBusiness<Reference>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentReference = await _roboticDataContext.References.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentReference == null)
      {
        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentReference);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Reference>
      {
        Message = "Referans Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    public async Task<ResultObjectBusiness<Reference>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentReference = await _roboticDataContext.References.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentReference == null)
      {
        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentReference.IsActive = false;
      currentReference.IsDeleted = true;

      _roboticDataContext.Update(currentReference);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Reference>
      {
        Message = "Referans Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Reference>> Update(Reference model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentReference = await _roboticDataContext.References.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentReference.Defination = model.Defination;
        currentReference.ImageFileName = model.ImageFileName;

        _roboticDataContext.Update(currentReference);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Reference>
          {
            Message = "Referans Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Reference>
        {
          Message = "Referans Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }
}
