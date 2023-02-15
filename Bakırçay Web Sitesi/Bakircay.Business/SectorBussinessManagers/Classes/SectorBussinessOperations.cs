using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.SectorBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.AppClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.SectorClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.SectorBussinessManagers.Classes;

public class SectorBussinessOperations : ISectorBussinessOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;
  public SectorBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticSectorBussiness");
  }

  public async Task<ResultObjectBusiness<Sector>> Add(Sector model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      await _roboticDataContext.Sectors.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Sector>
        {
          Message = "Sektör Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<Sector>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentSector = await _roboticDataContext.Sectors.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentSector.IsActive = !currentSector.IsActive;


    _roboticDataContext.Update(currentSector);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Güncelleme Başarılı",
        ResultObject = currentSector,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<Sector>
    {
      Message = "Sektör Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentSector,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };

  }

  public async Task<ResultObjectBusiness<Sector>> GetActiveSectorList()
  {
    var currentApps =
      await _roboticDataContext.Sectors.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

    return new ResultObjectBusiness<Sector>
    {
      ResultObjects = currentApps
    };
  }

  public async Task<ResultObjectBusiness<Sector>> GetAll()
  {
    var sectorList = await _roboticDataContext.Sectors.Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in sectorList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<Sector>
    {
      Message = string.Empty,
      ResultObjects = sectorList
    };
  }

  public async Task<ResultObjectBusiness<Sector>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentSector = await _roboticDataContext.Sectors.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<Sector>
    {
      ResultObject = currentSector
    };

  }

  public async Task<ResultObjectBusiness<Sector>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentSector = await _roboticDataContext.Sectors.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentSector == null)
    {
      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentSector);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<Sector>
    {
      Message = "Sektör Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<Sector>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentSector = await _roboticDataContext.Sectors.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentSector == null)
    {
      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentSector.IsActive = false;
    currentSector.IsDeleted = true;

    _roboticDataContext.Update(currentSector);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<Sector>
    {
      Message = "Sektör Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };

  }

  public async Task<ResultObjectBusiness<Sector>> Update(Sector model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      var currentSector = await _roboticDataContext.Sectors.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentSector.DefinationTr = model.DefinationTr;
      currentSector.DefinationEn = model.DefinationEn;

      _roboticDataContext.Update(currentSector);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Sector>
        {
          Message = "Sektör Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Sector>
      {
        Message = "Sektör Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
}