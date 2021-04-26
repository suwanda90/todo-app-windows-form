using ApplicationCore.Entities;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<GroupTask> GroupTask { get; private set; }
        public DbSet<Tasks> Tasks { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var clrType = property.ClrType;
                    var isPrimaryKey = property.IsPrimaryKey();
                    var isForeignKey = property.IsForeignKey();

                    if (isPrimaryKey && clrType == typeof(Guid))
                    {
                        property.SetDefaultValueSql("NEWID()");
                    }
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.SetNull;
            }
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess: true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IAuditableEntity)entity.Entity).DateCreated = DateTime.Now.ToLocalTime();
                    ((IAuditableEntity)entity.Entity).DateModified = DateTime.Now.ToLocalTime();
                }
                else
                {
                    foreach (var prop in entity.Properties)
                    {
                        if (prop.Metadata.Name == "DateCreated" || prop.Metadata.Name == "CreatedBy")
                        {
                            prop.IsModified = false;
                        }
                    }

                    ((IAuditableEntity)entity.Entity).DateModified = DateTime.Now.ToLocalTime();
                }
            }
        }
    }
}
