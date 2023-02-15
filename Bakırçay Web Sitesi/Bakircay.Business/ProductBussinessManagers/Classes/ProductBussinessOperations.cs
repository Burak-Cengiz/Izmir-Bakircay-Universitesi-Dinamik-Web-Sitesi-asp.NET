using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.AppClasses;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Helper.Helper;

namespace R1Robotic.Business.ProductBussinessManagers.Classes;
public class ProductBussinessOperations : IProductBussinessOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ProductBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductBussiness");
  }
  public async Task<ResultObjectBusiness<Product>> Add(Product model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.NameTr);
      model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.NameEn);

      await _roboticDataContext.Products.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        model.EnchKey = _dataProtector.Protect(model.Id.ToString());

        return new ResultObjectBusiness<Product>
        {
          Message = "Ürün Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = $"/Product/Update/{model.EnchKey}"
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<Product>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProduct = await _roboticDataContext.Products.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentProduct.IsActive = !currentProduct.IsActive;


    _roboticDataContext.Update(currentProduct);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Güncelleme Başarılı",
        ResultObject = currentProduct,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<Product>
    {
      Message = "Ürün Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentProduct,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<Product>> GetActiveProductList()
  {
    var productList = await _roboticDataContext.Products.Include(t => t.ProductAppScope).Include(t => t.ProductBrand)
      .Include(t => t.ProductCategory).Include(t => t.ProductMountingMeth).Include(t => t.ProductFeatures.Where(x => !x.IsDeleted && x.IsActive)).ThenInclude(t => t.Feature)
      .Include(t => t.ProductImages.Where(x => x.IsActive && !x.IsDeleted)).Where(t => t.IsActive && !t.IsDeleted).ToListAsync();


    return new ResultObjectBusiness<Product>
    {
      ResultObjects = productList
    };
  }

  public async Task<ResultObjectBusiness<Product>> GetByUrl(string url)
  {
    var currentProduct =
      await _roboticDataContext.Products.Include(t => t.ProductAppScope).Include(t => t.ProductCategory).Include(t => t.ProductBrand).Include(t => t.ProductFeatures).ThenInclude(t => t.Feature).Include(t => t.ProductImages).FirstOrDefaultAsync(t =>
        t.UrlTr == url || t.UrlEn == url && !t.IsDeleted && t.IsActive);

    return new ResultObjectBusiness<Product>
    {
      ResultObject = currentProduct
    };
  }

  public async Task<ResultObjectBusiness<Product>> GetAll()
  {
    var appList = await _roboticDataContext.Products.Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in appList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<Product>
    {
      Message = string.Empty,
      ResultObjects = appList
    };
  }

  public async Task<ResultObjectBusiness<Product>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProduct = await _roboticDataContext.Products.Include(t => t.ProductFeatures.Where(x => !x.IsDeleted)).ThenInclude(t => t.Feature).Include(t => t.ProductImages).FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<Product>
    {
      ResultObject = currentProduct
    };
  }

  public async Task<ResultObjectBusiness<Product>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProduct = await _roboticDataContext.Products.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProduct == null)
    {
      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentProduct);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<Product>
    {
      Message = "Ürün Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<Product>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProduct = await _roboticDataContext.Products.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProduct == null)
    {
      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProduct.IsActive = false;
    currentProduct.IsDeleted = true;

    _roboticDataContext.Update(currentProduct);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<Product>
    {
      Message = "Ürün Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<Product>> Update(Product model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {

      var currentProduct = await _roboticDataContext.Products.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentProduct.NameTr = model.NameTr;
      currentProduct.NameEn = model.NameEn;
      currentProduct.ContentTr = model.ContentTr;
      currentProduct.ContentEn = model.ContentEn;
      currentProduct.TechnicDetailsTr = model.TechnicDetailsTr;
      currentProduct.TechnicDetailsEn = model.TechnicDetailsEn;

      currentProduct.PageTitleTr = model.PageTitleTr;
      currentProduct.PageTitleEn = model.PageTitleEn;

      currentProduct.PageDescriptionTr = model.PageDescriptionTr;
      currentProduct.PageDescriptionEn = model.PageDescriptionEn;

      currentProduct.PageKeywordsTr = model.PageKeywordsTr;
      currentProduct.PageKeywordsEn = model.PageKeywordsEn;

      currentProduct.ProductCategoryId = model.ProductCategoryId;
      model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.NameTr);
      model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.NameEn);


      _roboticDataContext.Update(currentProduct);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Product>
        {
          Message = "Ürün Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Product>
      {
        Message = "Ürün Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
}