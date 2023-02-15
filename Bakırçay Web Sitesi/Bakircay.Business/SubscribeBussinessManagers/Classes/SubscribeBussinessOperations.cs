using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.SubscribeBusinessManagers.Interface;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProjectClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.Subscribe;
using R1Robotic.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R1Robotic.Business.SubscribeBusinessManagers.Classes
{
  public class SubscribeBussinessOperations : ISubscribeBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public SubscribeBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticSubscribeBusiness");
    }

    public async Task<ResultObjectBusiness<Subscribe>> Add(Subscribe model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();


      try
      {
        await _roboticDataContext.Subscribes.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Subscribe>
          {
            Message = "Abone Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Proje Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

    }

    public async Task<ResultObjectBusiness<Subscribe>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSubscribe = await _roboticDataContext.Subscribes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentSubscribe.IsActive = !currentSubscribe.IsActive;


      _roboticDataContext.Update(currentSubscribe);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Güncelleme Başarılı",
          ResultObject = currentSubscribe,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Subscribe>
      {
        Message = "Abone Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentSubscribe,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Subscribe>> GetAll()
    {
      var SubscribetList = await _roboticDataContext.Subscribes.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in SubscribetList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Subscribe>
      {
        Message = string.Empty,
        ResultObjects = SubscribetList
      };
    }

    public async Task<ResultObjectBusiness<Subscribe>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSubscribe = await _roboticDataContext.Subscribes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Subscribe>
      {
        ResultObject = currentSubscribe
      };

    }

    public async Task<ResultObjectBusiness<Subscribe>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSubscribe = await _roboticDataContext.Subscribes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentSubscribe == null)
      {
        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentSubscribe);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Subscribe>
      {
        Message = "Abone Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Subscribe>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSubscribe = await _roboticDataContext.Subscribes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentSubscribe == null)
      {
        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentSubscribe.IsActive = false;
      currentSubscribe.IsDeleted = true;

      _roboticDataContext.Update(currentSubscribe);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Subscribe>
      {
        Message = "Abone Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Subscribe>> Update(Subscribe model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentProject = await _roboticDataContext.Subscribes.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentProject.Email = model.Email;

        _roboticDataContext.Update(currentProject);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Subscribe>
          {
            Message = "Abone Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Subscribe>
        {
          Message = "Abone Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }
}
