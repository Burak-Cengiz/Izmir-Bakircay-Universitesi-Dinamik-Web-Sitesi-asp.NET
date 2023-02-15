using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.ProductBussinessManagers.Classes;

public class ProductImageBussinessOperations : IProductImageBussinessOperations
{
  private readonly RoboticDataContext _roboticDataContext;
  private readonly IDataProtector _dataProtector;

  public ProductImageBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
  {
    _roboticDataContext = roboticDataContext;
    _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductImageBussiness");
  }
  public async Task<ResultObjectBusiness<ProductImage>> Add(ProductImage model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      await _roboticDataContext.ProductImages.AddAsync(model);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductImage>
        {
          Message = "Ürün Resmi Ekleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Ekleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
  public async Task<ResultObjectBusiness<ProductImage>> ChangeStatus(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductImage = await _roboticDataContext.ProductImages.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    currentProductImage.IsActive = !currentProductImage.IsActive;


    _roboticDataContext.Update(currentProductImage);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Güncelleme Başarılı",
        ResultObject = currentProductImage,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductImage>
    {
      Message = "Ürün Resmi Güncelleme Sırasında Hata Oluştu",
      ResultObject = currentProductImage,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }

  public async Task<ResultObjectBusiness<ProductImage>> GetProductImageList(int productId)
  {
    var productImageList = await _roboticDataContext.ProductImages.Where(t => !t.IsDeleted && t.ProductId == productId).ToListAsync();

    return new ResultObjectBusiness<ProductImage>
    {
      Message = string.Empty,
      ResultObjects = productImageList
    };
  }

  public async Task<bool> Delete(int id)
  {
    var productImage = await _roboticDataContext.ProductImages.FirstOrDefaultAsync(t => t.Id == id);

    _roboticDataContext.ProductImages.Remove(productImage);
    await _roboticDataContext.SaveChangesAsync();

    return true;
  }

  public async Task<ResultObjectBusiness<ProductImage>> GetAll()
  {
    var productImageList = await _roboticDataContext.ProductImages.Where(t => !t.IsDeleted).ToListAsync();

    foreach (var item in productImageList)
      item.EnchKey = _dataProtector.Protect(item.Id.ToString());

    return new ResultObjectBusiness<ProductImage>
    {
      Message = string.Empty,
      ResultObjects = productImageList
    };
  }
  public async Task<ResultObjectBusiness<ProductImage>> GetById(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductImage = await _roboticDataContext.ProductImages.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    return new ResultObjectBusiness<ProductImage>
    {
      ResultObject = currentProductImage
    };
  }
  public async Task<ResultObjectBusiness<ProductImage>> HardDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductImage = await _roboticDataContext.ProductImages.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductImage == null)
    {
      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    _roboticDataContext.Remove(currentProductImage);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductImage>
    {
      Message = "Ürün Resmi Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };

  }
  public async Task<ResultObjectBusiness<ProductImage>> SoftDelete(string id)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    var currentProductImage = await _roboticDataContext.ProductImages.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

    if (currentProductImage == null)
    {
      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Bulunamadı",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    currentProductImage.IsActive = false;
    currentProductImage.IsDeleted = true;

    _roboticDataContext.Update(currentProductImage);

    var result = await _roboticDataContext.SaveChangesAsync();

    if (result > 0)
    {
      await transaction.CommitAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Silme Başarılı",
        ResultObject = null,
        ResultStatus = ResultStatus.Success,
        Url = string.Empty
      };
    }

    await transaction.RollbackAsync();

    return new ResultObjectBusiness<ProductImage>
    {
      Message = "Ürün Resmi Silme Sırasında Hata Oluştu",
      ResultObject = null,
      ResultStatus = ResultStatus.Error,
      Url = string.Empty
    };
  }
  public async Task<ResultObjectBusiness<ProductImage>> Update(ProductImage model)
  {
    await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

    try
    {
      var currentProductImage = await _roboticDataContext.ProductImages.FirstOrDefaultAsync(t => t.Id == model.Id);

      currentProductImage.ProductId = model.ProductId;
      currentProductImage.Product = model.Product;
      currentProductImage.ImagePath = model.ImagePath;

      _roboticDataContext.Update(currentProductImage);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductImage>
        {
          Message = "Ürün Resmi Güncelleme Başarılı",
          ResultObject = model,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
    catch (Exception e)
    {
      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductImage>
      {
        Message = "Ürün Resmi Güncelleme Sırasında Hata Oluştu",
        ResultObject = model,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }
  }
}