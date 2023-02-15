using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Entity.ViewModels;

namespace R1Robotic.Business.ProductBussinessManagers.Classes;

public class ProductFeatureBussinessOperations : IProductFeatureBussinessOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ProductFeatureBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductFeatureBussiness");
  }

  public async Task<ResultObjectBusiness<ProductFeature>> Add(ProductFeature model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {

      await _roboticDataContext.ProductFeatures.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductFeature>
        {
          Message = "Ürün Özelliği Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<ProductFeature>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentProductFeature.IsActive = !currentProductFeature.IsActive;


    _roboticDataContext.Update(currentProductFeature);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Güncelleme Başarılı",
        ResultObject = currentProductFeature,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductFeature>
    {
      Message = "Ürün Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentProductFeature,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> GetAll()
  {
    var appList = await _roboticDataContext.ProductFeatures.Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in appList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<ProductFeature>
    {
      Message = string.Empty,
      ResultObjects = appList
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<ProductFeature>
    {
      ResultObject = currentProductFeature
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductFeature == null)
    {
      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentProductFeature);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductFeature>
    {
      Message = "Ürün Özeliği Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductFeature == null)
    {
      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductFeature.IsActive = false;
    currentProductFeature.IsDeleted = true;

    _roboticDataContext.Update(currentProductFeature);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductFeature>
    {
      Message = "Ürün Özelliği Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> Update(ProductFeature model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentProductFeature.FeatureId = model.FeatureId;
      currentProductFeature.ProductId = model.ProductId;
      currentProductFeature.Value = model.Value;

      _roboticDataContext.Update(currentProductFeature);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductFeature>
        {
          Message = "Ürün Özelliği Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }

  public async Task<ResultObjectBusiness<ProductFeature>> GetProductFeatureById(int id)
  {
    return new ResultObjectBusiness<ProductFeature>
    {
      ResultObject = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == id)
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> GetProductFeaturesNonDeletedAndActiveProductId(int id)
  {
    return new ResultObjectBusiness<ProductFeature>
    {
      ResultObjects = await _roboticDataContext.ProductFeatures.Include(t => t.Feature).Where(t => t.ProductId == id && !t.IsDeleted).ToListAsync()
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> Delete(int id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == id);

    if (currentProductFeature == null)
    {
      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductFeature.IsDeleted = true;
    currentProductFeature.IsActive = false;

    _roboticDataContext.Update(currentProductFeature);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Silme Başarılı",
        ResultObject = currentProductFeature,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductFeature>
    {
      Message = "Ürün Özeliği Silme Sırasında Hata Oluştu",
      ResultObject = currentProductFeature,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductFeature>> ChangeBaseStatus(int id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductFeature = await _roboticDataContext.ProductFeatures.FirstOrDefaultAsync(t => t.Id == id);

    if (currentProductFeature == null)
    {
      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductFeature.IsActive = !currentProductFeature.IsActive;


    _roboticDataContext.Update(currentProductFeature);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductFeature>
      {
        Message = "Ürün Özelliği Durum Değiştirme Başarılı",
        ResultObject = currentProductFeature,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductFeature>
    {
      Message = "Ürün Özeliği Durum Değiştirme Sırasında Hata Oluştu",
      ResultObject = currentProductFeature,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<List<ProductFeaturesViewModel>> GetFeaturedForFilter()
  {
    List<ProductFeaturesViewModel> lstFeaturesViewModels = new List<ProductFeaturesViewModel>();

    var features = await _roboticDataContext.Features.Where(t => t.IsActive && !t.IsDeleted).ToListAsync();

    foreach (var feature in features)
    {
      if (!_roboticDataContext.ProductFeatures.Any(t => t.FeatureId == feature.Id))
        continue;

      var productFeatureMin =
        _roboticDataContext.ProductFeatures.Where(t => t.FeatureId == feature.Id).Min(t => t.Value);

      var productFeatureMax =
        _roboticDataContext.ProductFeatures.Where(t => t.FeatureId == feature.Id).Max(t => t.Value);

      lstFeaturesViewModels.Add(new ProductFeaturesViewModel
      {
        Feature = feature,
        MinValue = productFeatureMin,
        MaxValue = productFeatureMax
      });
    }

    return lstFeaturesViewModels;
  }

  public async Task<List<Product>> GetProductList(ProductFeaturesViewModel model)
  {
    var productFeatures = await _roboticDataContext.ProductFeatures.Include(t => t.Product).ThenInclude(t => t.ProductFeatures).ThenInclude(t => t.Feature).Include(t => t.Product).ThenInclude(t => t.ProductImages)
      .Where(t => t.FeatureId == model.FeatureId).ToListAsync();

    productFeatures = productFeatures.Where(t => t.Value >= model.MinValue && t.Value <= model.MaxValue).ToList();

    List<Product> lstProduct = new List<Product>();


    foreach (var item in productFeatures)
    {
      if (lstProduct.Any(t => t.Id == item.ProductId))
        continue;

      lstProduct.Add(item.Product);
    }

    return lstProduct;
  }

  public async Task<bool> CheckExistProductFeature(ProductFeature model)
  {
    return await _roboticDataContext.ProductFeatures.AnyAsync(t => !t.IsDeleted && t.ProductId == model.ProductId && t.FeatureId == model.FeatureId);
  }

  public async Task<bool> CheckExistProductFeatureUpdate(ProductFeature model)
  {
    return await _roboticDataContext.ProductFeatures.AnyAsync(t => !t.IsDeleted && t.ProductId == model.ProductId && t.FeatureId == model.FeatureId && t.Id != model.Id);
  }
}