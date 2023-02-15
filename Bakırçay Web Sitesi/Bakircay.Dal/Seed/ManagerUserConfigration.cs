using Bakircay.Entity.Classes.UserClasses;
using Bakircay.Helper.EncryptHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakircay.Dal.Seed
{
  public class ManagerUserConfigration : IEntityTypeConfiguration<ManagerUser>
  {
    public void Configure(EntityTypeBuilder<ManagerUser> builder)
    {
      builder.HasData(new ManagerUser
      {
        LastModifiedBy = "Seed",
        CreatedBy = "Seed",
        CreatedOn = DateTime.Now,
        EMail = "k@k.com",
        Password = Md5Helper.EncryptPassword("12345"),
        IsActive = true,
        IsDeleted = false,
        LastModifiedOn = DateTime.Now,
        Name = "Kağan",
        SurName = "Azar",
        Id = 1
      });
    }
  }
}
