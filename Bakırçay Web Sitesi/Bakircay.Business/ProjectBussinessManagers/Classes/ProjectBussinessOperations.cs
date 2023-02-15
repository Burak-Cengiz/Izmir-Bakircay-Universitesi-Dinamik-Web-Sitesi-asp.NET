using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProjectBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.AppClasses;
using R1Robotic.Entity.Classes.ProjectClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Helper.Helper;

namespace R1Robotic.Business.ProjectBussinessManagers.Classes
{
  public class ProjectBussinessOperations : IProjectBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public ProjectBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticProjectBussiness");
    }

    public async Task<ResultObjectBusiness<Project>> Add(Project model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        await _roboticDataContext.Projects.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Project>
          {
            Message = "Proje Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

    }

    public async Task<ResultObjectBusiness<Project>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProject = await _roboticDataContext.Projects.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentProject.IsActive = !currentProject.IsActive;


      _roboticDataContext.Update(currentProject);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Güncelleme Başarılı",
          ResultObject = currentProject,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Project>
      {
        Message = "Proje Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentProject,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Project>> GetAll()
    {
      var ProjectList = await _roboticDataContext.Projects.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in ProjectList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Project>
      {
        Message = string.Empty,
        ResultObjects = ProjectList
      };

    }

    public async Task<ResultObjectBusiness<Project>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProject = await _roboticDataContext.Projects.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Project>
      {
        ResultObject = currentProject
      };
    }

    public async Task<ResultObjectBusiness<Project>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProject = await _roboticDataContext.Projects.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentProject == null)
      {
        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentProject);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Project>
      {
        Message = "Proje Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Project>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProject = await _roboticDataContext.Projects.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentProject == null)
      {
        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentProject.IsActive = false;
      currentProject.IsDeleted = true;

      _roboticDataContext.Update(currentProject);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Project>
      {
        Message = "Proje Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }

    public async Task<ResultObjectBusiness<Project>> Update(Project model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentProject = await _roboticDataContext.Projects.FirstOrDefaultAsync(t => t.Id == model.Id);

        if (currentProject.DefinationTr != model.DefinationTr)
          currentProject.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);

        if (currentProject.DefinationEn != model.DefinationEn)
          currentProject.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        currentProject.DefinationTr = model.DefinationTr;
        currentProject.DefinationEn = model.DefinationEn;
        currentProject.ContentTr = model.ContentTr;
        currentProject.ContentEn = model.ContentEn;
        currentProject.ImagePath = model.ImagePath;
        currentProject.ShortDescTr = model.ShortDescTr;
        currentProject.ShortDescEn = model.ShortDescEn;


        _roboticDataContext.Update(currentProject);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Project>
          {
            Message = "Proje Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Project>
        {
          Message = "Proje Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

    }

    public async Task<ResultObjectBusiness<Project>> GetByUrl(string url)
    {
      var currentProject =
        await _roboticDataContext.Projects.FirstOrDefaultAsync(t =>
          t.UrlTr == url || t.UrlEn == url && !t.IsDeleted && t.IsActive);

      return new ResultObjectBusiness<Project>
      {
        ResultObject = currentProject
      };
    }

    public async Task<ResultObjectBusiness<Project>> GetActiveProjectList()
    {
      var currentProjects =
        await _roboticDataContext.Projects.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

      return new ResultObjectBusiness<Project>
      {
        ResultObjects = currentProjects
      };
    }
  }
}
