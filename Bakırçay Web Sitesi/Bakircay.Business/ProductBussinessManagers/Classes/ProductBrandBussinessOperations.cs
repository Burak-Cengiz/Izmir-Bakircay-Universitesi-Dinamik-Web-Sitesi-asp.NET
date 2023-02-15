using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.ProductBussinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ProductClasses;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Enums;

namespace R1Robotic.Business.ProductBussinessManagers.Classes
{
  public class ProductBrandBussinessOperations : IProductBrandBussinessOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;
    public ProductBrandBussinessOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticProductBrandBussiness");
    }
    public async Task<ResultObjectBusiness<ProductBrand>> Add(ProductBrand model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.ProductBrands.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<ProductBrand>
          {
            Message = "Ürün Marka Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<ProductBrand>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProductBrand = await _roboticDataContext.ProductBrands.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentProductBrand.IsActive = !currentProductBrand.IsActive;


      _roboticDataContext.Update(currentProductBrand);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Güncelleme Başarılı",
          ResultObject = currentProductBrand,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductBrand>
      {
        Message = "Ürün Marka Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentProductBrand,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<ProductBrand>> GetActiveProductBrandList()
    {
      var productBrands =
        await _roboticDataContext.ProductBrands.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

      return new ResultObjectBusiness<ProductBrand>
      {
        ResultObjects = productBrands
      };
    }

    public async Task<ResultObjectBusiness<ProductBrand>> GetAll()
    {
      var ProductBrandList = await _roboticDataContext.ProductBrands.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in ProductBrandList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<ProductBrand>
      {
        Message = string.Empty,
        ResultObjects = ProductBrandList
      };
    }

    public async Task<ResultObjectBusiness<ProductBrand>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProductBrand = await _roboticDataContext.ProductBrands.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<ProductBrand>
      {
        ResultObject = currentProductBrand
      };
    }

    public async Task<ResultObjectBusiness<ProductBrand>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProductBrand = await _roboticDataContext.ProductBrands.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentProductBrand == null)
      {
        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentProductBrand);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductBrand>
      {
        Message = "Ürün Marka Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<ProductBrand>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentProductBrand = await _roboticDataContext.ProductBrands.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentProductBrand == null)
      {
        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentProductBrand.IsActive = false;
      currentProductBrand.IsDeleted = true;

      _roboticDataContext.Update(currentProductBrand);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<ProductBrand>
      {
        Message = "Ürün Marka Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<ProductBrand>> Update(ProductBrand model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentProductBrand = await _roboticDataContext.ProductBrands.FirstOrDefaultAsync(t => t.Id == model.Id);

        currentProductBrand.Defination = model.Defination;
    

        _roboticDataContext.Update(currentProductBrand);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<ProductBrand>
          {
            Message = "Ürün Marka Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<ProductBrand>
        {
          Message = "Ürün Marka Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
  }
}
