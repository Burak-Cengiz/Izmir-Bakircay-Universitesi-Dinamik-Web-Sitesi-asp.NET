using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.ProductBussinessManagers.Classes
{
  public class FeatureBussinessOperations : IFeatureBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;
    public FeatureBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticFeatureBussiness");
    }
    public async Task<ResultObjectBusiness<Feature>> Add(Feature model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.Features.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Feature>
          {
            Message = "Özellik Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

    }

    public async Task<ResultObjectBusiness<Feature>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFeature = await _roboticDataContext.Features.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentFeature.IsActive = !currentFeature.IsActive;


      _roboticDataContext.Update(currentFeature);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Güncelleme Başarılı",
          ResultObject = currentFeature,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Feature>
      {
        Message = "Özellik Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentFeature,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Feature>> GetActiveFeatureList()
    {
      var currentApps =
        await _roboticDataContext.Features.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

      return new ResultObjectBusiness<Feature>
      {
        ResultObjects = currentApps
      };
    }

    public async Task<ResultObjectBusiness<Feature>> GetAll()
    {
      var featureList = await _roboticDataContext.Features.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in featureList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Feature>
      {
        Message = string.Empty,
        ResultObjects = featureList
      };


    }

    public async Task<ResultObjectBusiness<Feature>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFeature = await _roboticDataContext.Features.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Feature>
      {
        ResultObject = currentFeature
      };

    }

    public async Task<ResultObjectBusiness<Feature>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFeature = await _roboticDataContext.Features.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentFeature == null)
      {
        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentFeature);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Feature>
      {
        Message = "Özellik Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Feature>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentFeature = await _roboticDataContext.Features.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentFeature == null)
      {
        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentFeature.IsActive = false;
      currentFeature.IsDeleted = true;

      _roboticDataContext.Update(currentFeature);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Feature>
      {
        Message = "Özellik Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Feature>> Update(Feature model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentFeature = await _roboticDataContext.Features.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentFeature.DescriptionTr = model.DescriptionTr;
        currentFeature.DescriptionEn = model.DescriptionEn;

        _roboticDataContext.Update(currentFeature);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Feature>
          {
            Message = "Özellik Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Feature>
        {
          Message = "Özellik Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }
}
