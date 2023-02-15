using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ManagerUserBusinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.UserClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Entity.ViewModels;
using R1Robotic.Helper.EncryptHelper;

namespace R1Robotic.Business.ManagerUserBusinessManagers.Classes;

internal class ManagerUserOperations : IManagerUserOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ManagerUserOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectorProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectorProvider.CreateProtector("RoboticManagerUserBussiness");
  }

  public async Task<ResultObjectBusiness<ManagerUser>> Add(ManagerUser model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      model.Password = Md5Helper.EncryptPassword(model.Password);

      await _roboticDataContext.ManagerUsers.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Kullanıcı Ekleme Başarılı",
          ResultObject = model,
          ResultObjects = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };

    }
  }

    
  public async Task<ResultObjectBusiness<ManagerUser>> Update(ManagerUser model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();
    try
    {
      var managerUser = await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(t => t.Id == model.Id);

      if (managerUser == null)
      {
        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Kullanıcı Bulunamadı",
          ResultObject = model,
          ResultObjects = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      managerUser.EMail = model.EMail;
      managerUser.Password = Md5Helper.EncryptPassword(model.Password);
      managerUser.Name = model.Name;
      managerUser.SurName = model.SurName;


      _roboticDataContext.Update(managerUser);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Kullanıcı Güncelleme Başarılı",
          ResultObject = model,
          ResultObjects = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultObjects = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultObjects = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<ManagerUser>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var managerUser =
      await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(
        t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (managerUser == null)
    {
      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Bulunamadı",
        ResultStatus = ResultStatus.Warning,
        Url = string.Empty
      };
    }

    try
    {

      managerUser.IsActive = false;
      managerUser.IsDeleted = true;

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Kullanıcı Silme Başarılı",
          ResultObject = managerUser,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Silme Sırasında Hata Oluştu",
        ResultObject = managerUser,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Silme Sırasında Hata Oluştu",
        ResultObject = managerUser,
        ResultObjects = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<ManagerUser>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      var managerUser =
        await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(
          t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (managerUser == null)
      {
        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Kullanıcı Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(managerUser);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Kullanıcı Silme Başarılı",
          ResultObject = managerUser,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<ManagerUser>> GetAll()
  {
    return new ResultObjectBusiness<ManagerUser>
    {
      ResultObjects = await _roboticDataContext.ManagerUsers.ToListAsync()
    };
  }

  public async Task<ResultObjectBusiness<ManagerUser>> GetById(string id)
  {
    var managerUser =
      await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(
        t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<ManagerUser>
    {
      ResultObject = managerUser
    };
  }

  public async Task<ResultObjectBusiness<ManagerUser>> ChangeStatus(string id)
  {
    var managerUser =
      await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(
        t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (managerUser == null)
    {
      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Bulunamadı",
        ResultStatus = ResultStatus.Error
      };
    }

    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      managerUser.IsActive = !managerUser.IsActive;

      _roboticDataContext.Update(managerUser);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ManagerUser>
        {
          Message = "Durum Değiştirme Başarılı",
          ResultObject = managerUser,
          ResultObjects = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Durum Değiştirme Sırasında Hata Oluştu",
        ResultObject = managerUser,
        ResultObjects = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Durum Değiştirme Sırasında Hata Oluştu",
        ResultObject = managerUser,
        ResultObjects = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

  }

  public async Task<ResultObjectBusiness<ManagerUser>> GetNonDeletedUser()
  {
    return new ResultObjectBusiness<ManagerUser>
    {
      ResultObjects = await _roboticDataContext.ManagerUsers.Where(t => !t.IsDeleted).ToListAsync()
    };
  }

  public async Task<ResultObjectBusiness<ManagerUser>> GetSecuritiedUserList()
  {
    var managerUsers = await _roboticDataContext.ManagerUsers.Where(t => !t.IsDeleted).ToListAsync();

    managerUsers.ForEach(x => x.ManagerUserEncrypedId = _dataProtector.Protect(x.Id.ToString()));

    return new ResultObjectBusiness<ManagerUser>
    {
      ResultObjects = managerUsers
    };

  }

  public async Task<ResultObjectBusiness<ManagerUser>> Login(LoginViewModel model)
  {
    var managerUser = await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(t => t.EMail == model.EMail && t.Password == Md5Helper.EncryptPassword(model.Password));

    if (managerUser == null)
    {
      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Bulunamadı",
        ResultStatus = ResultStatus.Error
      };
    }

    return new ResultObjectBusiness<ManagerUser>
    {
      Message = string.Empty,
      ResultStatus = ResultStatus.Success,
      ResultObject = managerUser
    };
  }

  public async Task<ResultObjectBusiness<ManagerUser>> ManagerUserFind(string id)
  {
    var managerUser =
      await _roboticDataContext.ManagerUsers.FirstOrDefaultAsync(
        t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (managerUser == null)
    {
      return new ResultObjectBusiness<ManagerUser>
      {
        Message = "Kullanıcı Bulunamadı",
        ResultStatus = ResultStatus.Error
      };
    }

    managerUser.Password = Md5Helper.DecryptPassword(managerUser.Password);
    managerUser.PasswordRepeat = managerUser.Password;

    return new ResultObjectBusiness<ManagerUser>
    {
      Message = string.Empty,
      ResultStatus = ResultStatus.Success,
      ResultObject = managerUser
    };
  }
}