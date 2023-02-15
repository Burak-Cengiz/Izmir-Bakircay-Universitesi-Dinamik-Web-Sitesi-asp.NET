using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ContactBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ContactClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.ContactBussinessManagers.Classes;

internal class SocialMediaAccountBussinessOperations : ISocialMediaAccountBussinessOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public SocialMediaAccountBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticSocialMediaAccountBusiness");
  }
  public async Task<ResultObjectBusiness<SocialMediaAccount>> Add(SocialMediaAccount model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      await _roboticDataContext.SocialMediaAccounts.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Hesap Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        Message = "Sosyal Medya Hesap Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        Message = "Sosyal Medya Hesap Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<SocialMediaAccount>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentSocialMediaAccount = await _roboticDataContext.SocialMediaAccounts.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentSocialMediaAccount.IsActive = !currentSocialMediaAccount.IsActive;


    _roboticDataContext.Update(currentSocialMediaAccount);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        Message = "Sosyal Medya Hesap Güncelleme Başarılı",
        ResultObject = currentSocialMediaAccount,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<SocialMediaAccount>
    {
      Message = "Sosyal Medya Hesap Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentSocialMediaAccount,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public SocialMediaAccount GetSocialMediaAccount()
  {
    return _roboticDataContext.SocialMediaAccounts.FirstOrDefault();
  }

  public async Task<bool> CheckSocialMediaExist()
  {
    var res = await _roboticDataContext.SocialMediaAccounts.AnyAsync();

    if (!res)
    {
      var newResult = await _roboticDataContext.AddAsync(new SocialMediaAccount
      {
        DefinationTr = "Sosyal Medya",
        DefinationEn = "Social Media",
        Facebook = "https://www.facebook.com",
        Instagram = "https://www.instagram.com",
        Twitter = "https://www.twitter.com",
        Youtube = "https://www.youtube.com"
      });

      _roboticDataContext.SaveChanges();

      return true;
    }
    return true;
  }

  public async Task<ResultObjectBusiness<SocialMediaAccount>> GetAll()
    {
      var SocialMediaAccountList = await _roboticDataContext.SocialMediaAccounts.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in SocialMediaAccountList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        Message = string.Empty,
        ResultObjects = SocialMediaAccountList
      };
    }

    public async Task<ResultObjectBusiness<SocialMediaAccount>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSocialMediaAccount = await _roboticDataContext.SocialMediaAccounts.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        ResultObject = currentSocialMediaAccount
      };
    }

    public async Task<ResultObjectBusiness<SocialMediaAccount>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSocialMediaAccount = await _roboticDataContext.SocialMediaAccounts.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentSocialMediaAccount == null)
      {
        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Hesap Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      _roboticDataContext.Remove(currentSocialMediaAccount);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        Message = "Sosyal Medya Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<SocialMediaAccount>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentSocialMediaAccount = await _roboticDataContext.SocialMediaAccounts.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentSocialMediaAccount == null)
      {
        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Hesap Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentSocialMediaAccount.IsActive = false;
      currentSocialMediaAccount.IsDeleted = true;

      _roboticDataContext.Update(currentSocialMediaAccount);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Hesap Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<SocialMediaAccount>
      {
        Message = "Sosyal Medya Hesap Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<SocialMediaAccount>> Update(SocialMediaAccount model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentSocialMediaAccount = await _roboticDataContext.SocialMediaAccounts.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentSocialMediaAccount.DefinationTr = model.DefinationTr;
        currentSocialMediaAccount.DefinationEn = model.DefinationEn;


        _roboticDataContext.Update(currentSocialMediaAccount);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<SocialMediaAccount>
          {
            Message = "Sosyal Medya Hesap  Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Hesap  Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<SocialMediaAccount>
        {
          Message = "Sosyal Medya Hesap  Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }