using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
namespace R1Robotic.Business.ProductBussinessManagers.Classes;

public class ProductMountingMethBussinessOperations : IProductMountingMethBussinessOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ProductMountingMethBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductMountingMethBussiness");
  }

  public async Task<ResultObjectBusiness<ProductMountingMeth>> Add(ProductMountingMeth model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      await _roboticDataContext.ProductMountingMeths.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductMountingMeth>
        {
          Message = "Ürün Montaj Şekli Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
  public async Task<ResultObjectBusiness<ProductMountingMeth>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductMountingMeth = await _roboticDataContext.ProductMountingMeths.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentProductMountingMeth.IsActive = !currentProductMountingMeth.IsActive;


    _roboticDataContext.Update(currentProductMountingMeth);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Güncelleme Başarılı",
        ResultObject = currentProductMountingMeth,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductMountingMeth>
    {
      Message = "Ürün Montaj Şekli Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentProductMountingMeth,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductMountingMeth>> GetActiveProductMountingMethList()
  {
    var productAppScopes =
      await _roboticDataContext.ProductMountingMeths.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

    return new ResultObjectBusiness<ProductMountingMeth>
    {
      ResultObjects = productAppScopes
    };
  }

  public async Task<ResultObjectBusiness<ProductMountingMeth>> GetAll()
  {
    var productMountingMethList = await _roboticDataContext.ProductMountingMeths.Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in productMountingMethList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<ProductMountingMeth>
    {
      Message = string.Empty,
      ResultObjects = productMountingMethList
    };
  }
  public async Task<ResultObjectBusiness<ProductMountingMeth>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductMountingMeth = await _roboticDataContext.ProductMountingMeths.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<ProductMountingMeth>
    {
      ResultObject = currentProductMountingMeth
    };
  }
  public async Task<ResultObjectBusiness<ProductMountingMeth>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductMountingMeth = await _roboticDataContext.ProductMountingMeths.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductMountingMeth == null)
    {
      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentProductMountingMeth);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductMountingMeth>
    {
      Message = "Ürün Montaj Şekli Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }
  public async Task<ResultObjectBusiness<ProductMountingMeth>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductMountingMeth = await _roboticDataContext.ProductMountingMeths.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductMountingMeth == null)
    {
      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductMountingMeth.IsActive = false;
    currentProductMountingMeth.IsDeleted = true;

    _roboticDataContext.Update(currentProductMountingMeth);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductMountingMeth>
    {
      Message = "Ürün Montaj Şekli Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }
  public async Task<ResultObjectBusiness<ProductMountingMeth>> Update(ProductMountingMeth model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      var currentProductMountingMeth = await _roboticDataContext.ProductMountingMeths.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentProductMountingMeth.DefinationTr = model.DefinationTr;
      currentProductMountingMeth.DefinationEn = model.DefinationEn;

      _roboticDataContext.Update(currentProductMountingMeth);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductMountingMeth>
        {
          Message = "Ürün Montaj Şekli Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductMountingMeth>
      {
        Message = "Ürün Montaj Şekli Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
}