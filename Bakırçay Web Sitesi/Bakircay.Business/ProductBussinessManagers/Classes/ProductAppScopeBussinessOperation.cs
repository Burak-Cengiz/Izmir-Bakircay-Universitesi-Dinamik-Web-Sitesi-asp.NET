using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.ProductBussinessManagers.Classes;

/// <summary>
/// Uygulama Alanları
/// </summary>
public class ProductAppScopeBussinessOperation : IProductAppScopeBussinessOperation
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ProductAppScopeBussinessOperation(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductAppScopeBussiness");
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> Add(ProductAppScope model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {

      await _roboticDataContext.ProductAppScopes.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductAppScope>
        {
          Message = "Uygulama alanı Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama alanı Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductAppScope = await _roboticDataContext.ProductAppScopes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentProductAppScope.IsActive = !currentProductAppScope.IsActive;

    _roboticDataContext.Update(currentProductAppScope);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı  Güncelleme Başarılı",
        ResultObject = currentProductAppScope,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductAppScope>
    {
      Message = "Uygulama Alanı Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentProductAppScope,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> GetActiveAppScopeList()
  {
    var productAppScopes =
      await _roboticDataContext.ProductAppScopes.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

    return new ResultObjectBusiness<ProductAppScope>
    {
      ResultObjects = productAppScopes
    };
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> GetAll()
  {
    var appList = await _roboticDataContext.ProductAppScopes.Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in appList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<ProductAppScope>
    {
      Message = string.Empty,
      ResultObjects = appList
    };
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductAppScope = await _roboticDataContext.ProductAppScopes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<ProductAppScope>
    {
      ResultObject = currentProductAppScope
    };
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductAppScope = await _roboticDataContext.ProductAppScopes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductAppScope == null)
    {
      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentProductAppScope);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductAppScope>
    {
      Message = "Uygulama Alanı Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductAppScope = await _roboticDataContext.ProductAppScopes.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductAppScope == null)
    {
      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductAppScope.IsActive = false;
    currentProductAppScope.IsDeleted = true;

    _roboticDataContext.Update(currentProductAppScope);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductAppScope>
    {
      Message = "Uygulama Alanı Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductAppScope>> Update(ProductAppScope model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      var currentProductAppScope = await _roboticDataContext.ProductAppScopes.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentProductAppScope.DefinationEn = model.DefinationEn;
      currentProductAppScope.DefinationTr = model.DefinationTr;


      _roboticDataContext.Update(currentProductAppScope);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductAppScope>
        {
          Message = "Uygulama Alanı Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductAppScope>
      {
        Message = "Uygulama Alanı Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
}