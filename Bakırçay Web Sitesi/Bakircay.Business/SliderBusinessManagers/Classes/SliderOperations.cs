using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using R1Robotic.Business.SliderBusinessManagers.Interfaces;
using R1Robotic.Dal.Context;
using R1Robotic.Entity.Classes.ResultObjectClasses;
using R1Robotic.Entity.Classes.SliderClasses;
using R1Robotic.Entity.Enums;
using R1Robotic.Helper.Helper;

namespace R1Robotic.Business.SliderBusinessManagers.Classes
{
  public class SliderOperations : ISliderOperations
  {
    private readonly RoboticDataContext _roboticDataContext;
    private readonly IDataProtector _dataProtector;

    public SliderOperations(RoboticDataContext roboticDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _roboticDataContext = roboticDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticSliderBussiness");
    }

    public async Task<ResultObjectBusiness<Slider>> Add(Slider model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        await _roboticDataContext.Sliders.AddAsync(model);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Slider>
          {
            Message = "Slider Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Slider>> Update(Slider model)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentSlider = await _roboticDataContext.Sliders.FirstOrDefaultAsync(t => t.Id == model.Id);

        if (currentSlider == null)
        {
          return new ResultObjectBusiness<Slider>
          {
            Message = "Slider Bulunamadı",
            ResultObject = model,
            ResultStatus = ResultStatus.Info,
            Url = string.Empty
          };
        }

        currentSlider.ContentEn = model.ContentEn;
        currentSlider.ContentTr = model.ContentTr;
        currentSlider.DefinationTr = model.DefinationTr;
        currentSlider.DefinationEn = model.DefinationEn;
        currentSlider.ImageUrlTr = model.ImageUrlTr;
        currentSlider.ImageUrlEn = model.ImageUrlEn;
        currentSlider.UrlTr = model.UrlTr;
        currentSlider.UrlEn = model.UrlEn;
        currentSlider.ButtonColor = model.ButtonColor;
        currentSlider.ButtonTextEn = model.ButtonTextEn;
        currentSlider.ButtonTextTr = model.ButtonTextTr;
        currentSlider.ContentColor = model.ContentColor;
        currentSlider.SliderFontSize = model.SliderFontSize;

        _roboticDataContext.Update(currentSlider);

        var result = await _roboticDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Slider>
          {
            Message = "Slider Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Slider>> SoftDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Sliders.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentApp == null)
      {
        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentApp.IsActive = false;
      currentApp.IsDeleted = true;

      _roboticDataContext.Update(currentApp);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Slider>
      {
        Message = "Slider Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Slider>> HardDelete(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Sliders.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentApp == null)
      {
        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _roboticDataContext.Remove(currentApp);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Slider Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Slider>
      {
        Message = "Slider Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Slider>> GetAll()
    {
      var SliderList = await _roboticDataContext.Sliders.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in SliderList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Slider>
      {
        Message = string.Empty,
        ResultObjects = SliderList
      };
    }

    public async Task<ResultObjectBusiness<Slider>> GetById(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Sliders.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Slider>
      {
        ResultObject = currentApp
      };
    }

    public async Task<ResultObjectBusiness<Slider>> ChangeStatus(string id)
    {
      await using var transaction = await _roboticDataContext.Database.BeginTransactionAsync();

      var currentApp = await _roboticDataContext.Sliders.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentApp.IsActive = !currentApp.IsActive;


      _roboticDataContext.Update(currentApp);

      var result = await _roboticDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Slider>
        {
          Message = "Durum Güncelleme Başarılı",
          ResultObject = currentApp,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Slider>
      {
        Message = "Durum Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentApp,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Slider>> GetActiveSliderList()
    {
      var sliders =
        await _roboticDataContext.Sliders.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

      return new ResultObjectBusiness<Slider>
      {
        ResultObjects = sliders
      };
    }
  }
}
