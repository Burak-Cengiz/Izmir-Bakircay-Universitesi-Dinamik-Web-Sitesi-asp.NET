using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ContactBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ContactClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R1Robotic.Business.ContactBussinessManagers.Classes
{
  public class SchoolAddressBussinessOperations : IFactoryAddressBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public SchoolAddressBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticFactoryAddressBussiness");
    }
    public async Task<ResultObjectBusiness<FactoryAddress>> Add(FactoryAddress model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.FactoryAddresses.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<FactoryAddress>
          {
            Message = "Fabrika Adres Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<FactoryAddress>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFactoryAddress = await _roboticDataContext.FactoryAddresses.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentFactoryAddress.IsActive = !currentFactoryAddress.IsActive;


      _roboticDataContext.Update(currentFactoryAddress);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Güncelleme Başarılı",
          ResultObject = currentFactoryAddress,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<FactoryAddress>
      {
        Message = "Fabrika Adres Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentFactoryAddress,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<FactoryAddress>> GetAll()
    {
      var factoryAddressList = await _roboticDataContext.FactoryAddresses.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in factoryAddressList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<FactoryAddress>
      {
        Message = string.Empty,
        ResultObjects = factoryAddressList
      };
    }

    public async Task<ResultObjectBusiness<FactoryAddress>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFactoryAddress = await _roboticDataContext.FactoryAddresses.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<FactoryAddress>
      {
        ResultObject = currentFactoryAddress
      };
    }

    public async Task<ResultObjectBusiness<FactoryAddress>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFactoryAddress = await _roboticDataContext.FactoryAddresses.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentFactoryAddress == null)
      {
        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentFactoryAddress);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<FactoryAddress>
      {
        Message = "Fabrika Adres Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<FactoryAddress>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFactoryAddress = await _roboticDataContext.FactoryAddresses.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentFactoryAddress == null)
      {
        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentFactoryAddress.IsActive = false;
      currentFactoryAddress.IsDeleted = true;

      _roboticDataContext.Update(currentFactoryAddress);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<FactoryAddress>
      {
        Message = "Fabrika Adres Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<FactoryAddress>> Update(FactoryAddress model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentFactoryAddress = await _roboticDataContext.FactoryAddresses.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentFactoryAddress.DefinationTr = model.DefinationTr;
        currentFactoryAddress.DefinationEn = model.DefinationEn;
        currentFactoryAddress.AdressTr = model.AdressTr;
        currentFactoryAddress.AdressEn = model.AdressEn;
        currentFactoryAddress.Location = model.Location;
        currentFactoryAddress.FacilityId = model.FacilityId;


        _roboticDataContext.Update(currentFactoryAddress);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<FactoryAddress>
          {
            Message = "Fabrika Adres Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<FactoryAddress>
        {
          Message = "Fabrika Adres Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }
}
