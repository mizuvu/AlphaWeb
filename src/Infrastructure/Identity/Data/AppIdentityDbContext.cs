using Application.Common.Interfaces;
using Domain.Database;
using Infrastructure.Auditing;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Zord.Core.Domain.Interfaces;

namespace Infrastructure.Identity.Data;

public class AppIdentityDbContext : IdentityDbContext
{
    private readonly ICurrentUser _currentUser;

    public AppIdentityDbContext(
        DbContextOptions<AppIdentityDbContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public virtual DbSet<ChangeEntryEntity> ChangeEntries => Set<ChangeEntryEntity>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();
        var auditEntries = OnBeforeSaveChanges();
        int result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChanges(auditEntries, cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ChangeEntryEntity>().ToTable(name: "ChangeEntries", Schema.Audit);

        builder.Entity<ApplicationUser>().ToTable(name: "Users", Schema.Identity);
        builder.Entity<IdentityUserLogin<string>>().ToTable(name: "UserLogins", Schema.Identity);
        builder.Entity<IdentityUserClaim<string>>().ToTable(name: "UserClaims", Schema.Identity);
        builder.Entity<IdentityUserToken<string>>().ToTable(name: "UserTokens", Schema.Identity);
        builder.Entity<ApplicationRole>().ToTable(name: "Roles", Schema.Identity);
        builder.Entity<IdentityRoleClaim<string>>().ToTable(name: "RoleClaims", Schema.Identity);
        builder.Entity<ApplicationUserRole>().ToTable(name: "UserRoles", Schema.Identity);

        builder.Entity<JwtToken>(e =>
        {
            e.HasKey(x => x.UserId);
            e.ToTable(name: "JwtTokens", Schema.Identity);
        });
    }

    private List<ChangeEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<ChangeEntry>();
        foreach (var entry in ChangeTracker.Entries().Where(e =>
            e.Entity.GetType() != typeof(ChangeEntryEntity) // fix savechanges when update claims
            && e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified))
        {
            var auditEntry = new ChangeEntry(entry)
            {
                UserId = _currentUser.UserId,
                TableName = entry.Entity.GetType().Name,
            };
            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.ActionType = ChangeType.Create;
                        auditEntry.NewValues[propertyName] = property!.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.ActionType = ChangeType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.ActionType = ChangeType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }

        //write logs only when DB changes
        foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties && _.ActionType != ChangeType.None))
        {
            ChangeEntries.Add(auditEntry.ToTrailLog());
        }

        return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
    }

    private Task OnAfterSaveChanges(List<ChangeEntry> auditEntries, CancellationToken cancellationToken = new CancellationToken())
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return Task.CompletedTask;

        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
            ChangeEntries.Add(auditEntry.ToTrailLog());
        }
        return SaveChangesAsync(cancellationToken);
    }

    private void AuditEntities()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTimeOffset.Now;
                    entry.Entity.CreatedBy = _currentUser.UserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTimeOffset.Now;
                    entry.Entity.LastModifiedBy = _currentUser.UserId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is IDeleteTracking softDelete)
                    {
                        softDelete.IsDeleted = true;
                        softDelete.DeletedOn = DateTimeOffset.Now;
                        softDelete.DeletedBy = _currentUser.UserId;
                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }
    }
}
