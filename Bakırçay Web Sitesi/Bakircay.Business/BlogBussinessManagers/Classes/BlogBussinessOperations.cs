using Bakircay.Business.BlogBussinessManagers.Interfaces;
using Bakircay.Dal.Context;
using Bakircay.Entity.Classes.BlogClasses;
using Bakircay.Entity.Classes.ResultObjectClasses;
using Bakircay.Entity.Enums;
using Bakircay.Helper.Helper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Bakircay.Business.BlogBussinessManagers.Classes
{
    public class BlogBussinessOperations : IBlogBussinessOperations
  {
    private readonly BakircayDataContext _bakircayDataContext;
    private readonly IDataProtector _dataProtector;

    public BlogBussinessOperations(BakircayDataContext bakircayDataContext, IDataProtectionProvider dataProtectionProvider)
    {
      _bakircayDataContext = bakircayDataContext;
      _dataProtector = dataProtectionProvider.CreateProtector("RoboticBlogBussinessManager");
    }

    public async Task<ResultObjectBusiness<Blog>> Add(Blog model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        model.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);
        model.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        await _bakircayDataContext.Blogs.AddAsync(model);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Blog>
          {
            Message = "Blog Ekleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Ekleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }

    public async Task<ResultObjectBusiness<Blog>> ChangeStatus(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentBlog = await _bakircayDataContext.Blogs.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      currentBlog.IsActive = !currentBlog.IsActive;


      _bakircayDataContext.Update(currentBlog);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Güncelleme Başarılı",
          ResultObject = currentBlog,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Blog>
      {
        Message = "Blog Güncelleme Sırasında Hata Oluştu",
        ResultObject = currentBlog,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Blog>> GetAll()
    {
      var BlogList = await _bakircayDataContext.Blogs.Where(t => !t.IsDeleted).ToListAsync();

      foreach (var item in BlogList)
        item.EnchKey = _dataProtector.Protect(item.Id.ToString());

      return new ResultObjectBusiness<Blog>
      {
        Message = string.Empty,
        ResultObjects = BlogList
      };
    }

    public async Task<ResultObjectBusiness<Blog>> GetById(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentBlog = await _bakircayDataContext.Blogs.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      return new ResultObjectBusiness<Blog>
      {
        ResultObject = currentBlog
      };
    }

    public async Task<ResultObjectBusiness<Blog>> HardDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentBlog = await _bakircayDataContext.Blogs.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentBlog == null)
      {
        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      _bakircayDataContext.Remove(currentBlog);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Blog>
      {
        Message = "Blog Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Blog>> SoftDelete(string id)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      var currentBlog = await _bakircayDataContext.Blogs.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32(_dataProtector.Unprotect(id)));

      if (currentBlog == null)
      {
        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Bulunamadı",
          ResultObject = null,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }

      currentBlog.IsActive = false;
      currentBlog.IsDeleted = true;

      _bakircayDataContext.Update(currentBlog);

      var result = await _bakircayDataContext.SaveChangesAsync();

      if (result > 0)
      {
        await transaction.CommitAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Silme Başarılı",
          ResultObject = null,
          ResultStatus = ResultStatus.Success,
          Url = string.Empty
        };
      }

      await transaction.RollbackAsync();

      return new ResultObjectBusiness<Blog>
      {
        Message = "Blog Silme Sırasında Hata Oluştu",
        ResultObject = null,
        ResultStatus = ResultStatus.Error,
        Url = string.Empty
      };
    }

    public async Task<ResultObjectBusiness<Blog>> Update(Blog model)
    {
      await using var transaction = await _bakircayDataContext.Database.BeginTransactionAsync();

      try
      {
        var currentBlog = await _bakircayDataContext.Blogs.FirstOrDefaultAsync(t => t.Id == model.Id);

        if (currentBlog.DefinationTr != model.DefinationTr)
          currentBlog.UrlTr = UrlHelper.TurkishChrToEnglishChr(model.DefinationTr);

        if (currentBlog.DefinationEn != model.DefinationEn)
          currentBlog.UrlEn = UrlHelper.TurkishChrToEnglishChr(model.DefinationEn);

        currentBlog.DefinationTr = model.DefinationTr;
        currentBlog.DefinationEn = model.DefinationEn;
        currentBlog.ContentTr = model.ContentTr;
        currentBlog.ContentEn = model.ContentEn;

        _bakircayDataContext.Update(currentBlog);

        var result = await _bakircayDataContext.SaveChangesAsync();

        if (result > 0)
        {
          await transaction.CommitAsync();

          return new ResultObjectBusiness<Blog>
          {
            Message = "Blog Güncelleme Başarılı",
            ResultObject = model,
            ResultStatus = ResultStatus.Success,
            Url = string.Empty
          };
        }

        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
      catch (Exception e)
      {
        await transaction.RollbackAsync();

        return new ResultObjectBusiness<Blog>
        {
          Message = "Blog Güncelleme Sırasında Hata Oluştu",
          ResultObject = model,
          ResultStatus = ResultStatus.Error,
          Url = string.Empty
        };
      }
    }
    public async Task<ResultObjectBusiness<Blog>> GetByUrl(string url)
    {
      var currentBlog =
        await _bakircayDataContext.Blogs.FirstOrDefaultAsync(t =>
          t.UrlTr == url || t.UrlEn == url && !t.IsDeleted && t.IsActive);

      return new ResultObjectBusiness<Blog>
      {
        ResultObject = currentBlog
      };
    }

    public async Task<ResultObjectBusiness<Blog>> GetActiveAppList()
    {
      var currentBlog =
        await _bakircayDataContext.Blogs.Where(t => !t.IsDeleted && t.IsActive).ToListAsync();

      return new ResultObjectBusiness<Blog>
      {
        ResultObjects = currentBlog
      };

    }
  }
}
