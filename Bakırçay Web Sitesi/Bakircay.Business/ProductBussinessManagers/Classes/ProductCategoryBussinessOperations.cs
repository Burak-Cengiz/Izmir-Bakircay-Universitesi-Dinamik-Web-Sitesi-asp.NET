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

public class ProductCategoryBussinessOperations : IProductCategoryBussinessOperations
{
  private string _def = string.Empty;
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ProductCategoryBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductCategoryBussiness");
  }

  public async Task<ResultObjectBusiness<ProductCategory>> Add(ProductCategory model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      if (model.ParentCategoryId == 0)
        model.ParentCategoryId = null;

      model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
      model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);
      await _roboticDataContext.ProductCategories.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductCategory>
        {
          Message = "Ürün Kategorisi Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
  public async Task<ResultObjectBusiness<ProductCategory>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductCategory = await _roboticDataContext.ProductCategories.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentProductCategory.IsActive = !currentProductCategory.IsActive;


    _roboticDataContext.Update(currentProductCategory);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Durum Güncelleme Başarılı",
        ResultObject = currentProductCategory,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductCategory>
    {
      Message = "Durum Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentProductCategory,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }
  public async Task<ResultObjectBusiness<ProductCategory>> GetAll()
  {
    var productCategoryList = await _roboticDataContext.ProductCategories.Include(t => t.ParentCategory).Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in productCategoryList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<ProductCategory>
    {
      Message = string.Empty,
      ResultObjects = productCategoryList
    };

  }
  public async Task<ResultObjectBusiness<ProductCategory>> List()
  {
    List<ProductCategory> items = new List<ProductCategory>();

    IList<ProductCategory> allCategories = await _roboticDataContext.ProductCategories.Where(t => !t.IsDeleted).ToListAsync();

    var parentCategories = allCategories.Where(c => c.ParentCategoryId == null).ToList();

    foreach (var cat in parentCategories)
    {
      items.Add(cat);

      GetSubTree(allCategories, cat, items);

    }

    foreach (var item in items)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<ProductCategory>
    {
      Message = string.Empty,
      ResultObjects = items
    };

  }

  public async Task<ResultObjectBusiness<ProductCategory>> GetActiveProductCategoryList()
  {
    var currentApps =
      await _roboticDataContext.ProductCategories.Include(t => t.Products).Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

    return new ResultObjectBusiness<ProductCategory>
    {
      ResultObjects = currentApps
    };
  }

  public async Task<ResultObjectBusiness<ProductCategory>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductCategory = await _roboticDataContext.ProductCategories.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<ProductCategory>
    {
      ResultObject = currentProductCategory
    };
  }
  public async Task<ResultObjectBusiness<ProductCategory>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductCategory = await _roboticDataContext.ProductCategories.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductCategory == null)
    {
      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentProductCategory);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductCategory>
    {
      Message = "Ürün kategorisi Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }
  public async Task<ResultObjectBusiness<ProductCategory>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductCategory = await _roboticDataContext.ProductCategories.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductCategory == null)
    {
      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductCategory.IsActive = false;
    currentProductCategory.IsDeleted = true;

    _roboticDataContext.Update(currentProductCategory);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductCategory>
    {
      Message = "Ürün Kategorisi Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }
  public async Task<ResultObjectBusiness<ProductCategory>> Update(ProductCategory model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      if (model.ParentCategoryId == 0)
        model.ParentCategoryId = null;

      var currentProductCategory = await _roboticDataContext.ProductCategories.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentProductCategory.ContentEn = model.ContentEn;
      currentProductCategory.ContentTr = model.ContentTr;
      currentProductCategory.DefinationTr = model.DefinationTr;
      currentProductCategory.DefinationEn = model.DefinationEn;
      currentProductCategory.ParentCategoryId = model.ParentCategoryId;
      currentProductCategory.PageDescription = model.PageDescription;
      currentProductCategory.PageTitle = model.PageTitle;
      currentProductCategory.PageKeywords = model.PageKeywords;

      currentProductCategory.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
      currentProductCategory.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

      _roboticDataContext.Update(currentProductCategory);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductCategory>
        {
          Message = "Ürün Kategorisi Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductCategory>
      {
        Message = "Ürün Kategorisi Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }


  private void GetSubTree(IList<ProductCategory> allCats, ProductCategory parent, IList<ProductCategory> items)
  {
    if (parent.ParentCategory != null)
      _def += parent.ParentCategory.DefinationTr + " > ";


    var subCats = allCats.Where(c => c.ParentCategoryId == parent.Id);

    foreach (var cat in subCats)
    {
      items.Add(new ProductCategory { Id = cat.Id, DefinationTr = _def + parent.DefinationTr + " > " + cat.DefinationTr, ContentEn = cat.ContentEn, ContentTr = cat.ContentTr, CreatedBy = cat.CreatedBy, CreatedOn = DateTime.Now, DefinationEn = cat.DefinationEn, IsActive = cat.IsActive, UrlTr = cat.UrlTr, IsDeleted = cat.IsDeleted, LastModifiedBy = cat.LastModifiedBy, LastModifiedOn = cat.LastModifiedOn, PageDescription = cat.PageDescription, PageKeywords = cat.PageKeywords, PageTitle = cat.PageKeywords, ParentCategoryId = cat.ParentCategoryId, UrlEn = cat.UrlEn });

      GetSubTree(allCats, cat, items);
    }
  }
}