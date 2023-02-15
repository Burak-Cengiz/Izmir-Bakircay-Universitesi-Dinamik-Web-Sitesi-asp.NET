using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ContactBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ContactClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.ContactBussinessManagers.Classes
{
  public class SchoolBussinessOperations : IFacilityBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public SchoolBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticFacilityBusiness");
    }
    public async Task<ResultObjectBusiness<Facility>> Add(Facility model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.Facilities.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Facility>
          {
            Message = "Tesis Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Facility>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFacility = await _roboticDataContext.Facilities.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentFacility.IsActive = !currentFacility.IsActive;


      _roboticDataContext.Update(currentFacility);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Güncelleme Başarılı",
          ResultObject = currentFacility,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Facility>
      {
        Message = "Tesis Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentFacility,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Facility>> GetAll()
    {
      var facilityList = await _roboticDataContext.Facilities.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in facilityList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Facility>
      {
        Message = string.Empty,
        ResultObjects = facilityList
      };
    }

    public async Task<ResultObjectBusiness<Facility>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFacility = await _roboticDataContext.Facilities.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Facility>
      {
        ResultObject = currentFacility
      };
    }

    public async Task<ResultObjectBusiness<Facility>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFacility = await _roboticDataContext.Facilities.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentFacility == null)
      {
        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentFacility);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Facility>
      {
        Message = "Tesis Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Facility>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFacility = await _roboticDataContext.Facilities.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentFacility == null)
      {
        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentFacility.IsActive = false;
      currentFacility.IsDeleted = true;

      _roboticDataContext.Update(currentFacility);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Facility>
      {
        Message = "Tesis Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Facility>> Update(Facility model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentFacility = await _roboticDataContext.Facilities.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentFacility.DefinationTr = model.DefinationTr;
        currentFacility.DefinationEn = model.DefinationEn;


        _roboticDataContext.Update(currentFacility);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Facility>
          {
            Message = "Tesis Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Facility>
        {
          Message = "Tesis Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

    }
  }
}
