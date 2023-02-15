using Bakircay.Entity.Classes.ContactClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakircay.Dal.Seed;

public class SocialMediaConfigration : IEntityTypeConfiguration<SocialMediaAccount>
{
  public void Configure(EntityTypeBuilder<SocialMediaAccount> builder)
  {
    builder.HasData(new SocialMediaAccount
    {
      LastModifiedBy = "Seed",
      CreatedBy = "Seed",
      CreatedOn = DateTime.Now,
      IsActive = true,
      IsDeleted = false,
      LastModifiedOn = DateTime.Now,
      Id = 1,
      DefinationTr = "Sosyal Medya",
      DefinationEn = "Social Media",
      Facebook = "https://www.facebook.com",
      Instagram = "https://www.instagram.com",
      Twitter = "https://www.twitter.com",
      Youtube = "https://www.youtube.com"
    });
  }
}