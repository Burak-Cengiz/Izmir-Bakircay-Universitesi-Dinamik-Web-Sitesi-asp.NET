using Bakircay.Dal.Seed;
using Bakircay.Entity.Classes.AppClasses;
using Bakircay.Entity.Classes.BaseClasses;
using Bakircay.Entity.Classes.BlogClasses;
using Bakircay.Entity.Classes.ContactClasses;
using Bakircay.Entity.Classes.CorporateClasses;
using Bakircay.Entity.Classes.ProductClasses;
using Bakircay.Entity.Classes.ProjectClasses;
using Bakircay.Entity.Classes.ReferencesClasses;
using Bakircay.Entity.Classes.SectorClasses;
using Bakircay.Entity.Classes.ServicesClasses;
using Bakircay.Entity.Classes.SiteClasses;
using Bakircay.Entity.Classes.SliderClasses;
using Bakircay.Entity.Classes.Subscribe;
using Bakircay.Entity.Classes.UserClasses;
using Bakircay.Entity.Classes.WebSiteMailClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bakircay.Dal.Context;

public class BakircayDataContext : DbContext
{

  #region Constructor

  public BakircayDataContext(DbContextOptions<BakircayDataContext> options) : base(options) { }

  #endregion

  #region DbSets

  public DbSet<App> Apps { get; set; }
  public DbSet<ContactEmail> ContactEmails { get; set; }
  public DbSet<School> Schools { get; set; }
  public DbSet<SchoolAddresses> SchoolAddresses { get; set; }
  public DbSet<PhoneNumber> PhoneNumbers { get; set; }
  public DbSet<SocialMediaAccount> SocialMediaAccounts { get; set; }
  public DbSet<Corporate> Corporations { get; set; }
  public DbSet<Reference> References { get; set; }
  public DbSet<Service> Services { get; set; }
  public DbSet<Feature> Features { get; set; }
  public DbSet<ManagerUser> ManagerUsers { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<ProductAppScope> ProductAppScopes { get; set; }
  public DbSet<ProductBrand> ProductBrands { get; set; }
  public DbSet<ProductCategory> ProductCategories { get; set; }
  public DbSet<ProductFeature> ProductFeatures { get; set; }
  public DbSet<ProductImage> ProductImages { get; set; }
  public DbSet<ProductMountingMeth> ProductMountingMeths { get; set; }
  public DbSet<Project> Projects { get; set; }
  public DbSet<Subscribe> Subscribes { get; set; }
  public DbSet<WebSiteMail> WebSiteMails { get; set; }
  public DbSet<Blog> Blogs { get; set; }
  public DbSet<BlogImage> BlogImages { get; set; }
  public DbSet<SiteSetting> SiteSettings { get; set; }
  public DbSet<Slider> Sliders { get; set; }
  public DbSet<Sector> Sectors { get; set; }

  #endregion

  #region Methods - Override

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new ManagerUserConfigration());
    //modelBuilder.ApplyConfiguration(new SocialMediaConfigration());

    base.OnModelCreating(modelBuilder);
  }

  public override int SaveChanges()
  {
    ChangeTracker.DetectChanges();

    var entries = ChangeTracker.Entries();

    SetBaseObjectValues(entries, string.Empty);

    return base.SaveChanges();
  }

  public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())

  {
    ChangeTracker.DetectChanges();

    var entries = ChangeTracker.Entries();

    SetBaseObjectValues(entries, string.Empty);
    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
  }

  private void SetBaseObjectValues(IEnumerable<EntityEntry> entries, string userName)
  {
    foreach (var entry in entries)
    {
      if (entry.Entity is BaseObject trackable)
      {
        var now = DateTime.Now;

        switch (entry.State)
        {
          case EntityState.Modified:
            trackable.LastModifiedOn = now;
            trackable.LastModifiedBy = userName;
            break;

          case EntityState.Added:
            trackable.CreatedOn = now;
            trackable.LastModifiedOn = now;
            trackable.CreatedBy = userName;
            trackable.LastModifiedBy = userName;
            trackable.IsActive = true;
            trackable.IsDeleted = false;
            break;
        }
      }
    }
  }

  #endregion

}