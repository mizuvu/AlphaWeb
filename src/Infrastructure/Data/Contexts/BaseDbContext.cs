using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Database;
using Domain.Entites;
using Infrastructure.Auditing;
using MediatR;
using Zord.Core.Domain.Interfaces;

namespace Infrastructure.Data.Contexts;

public abstract class BaseDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;
    private readonly IPublisher _publisher;

    public BaseDbContext(
        DbContextOptions options,
        ICurrentUser currentUser,
        IPublisher publisher)
        : base(options)
    {
        _currentUser = currentUser;
        _publisher = publisher;
    }

    public virtual DbSet<Notification> Notifications => Set<Notification>();
    public virtual DbSet<ChangeEntryEntity> ChangeEntries => Set<ChangeEntryEntity>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();

        var auditEntries = OnBeforeSaveChanges();
        int result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChanges(auditEntries, cancellationToken);

        await _publisher.DispatchDomainEvents(this);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ChangeEntryEntity>().ToTable(name: "ChangeEntries", Schema.Audit);
        builder.Entity<Notification>().ToTable(name: "Notifications", Schema.System);
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
