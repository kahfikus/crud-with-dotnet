using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Exam20203.Entities
{
    public partial class CartDbContext : DbContext
    {
        
        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbMakanan> TbMakanan { get; set; }
        public virtual DbSet<TbTransaction> TbTransaction { get; set; }
        public virtual DbSet<TbUser> TbUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbMakanan>(entity =>
            {
                entity.Property(e => e.MakananId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("'SYSTEM'::character varying");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedBy).HasDefaultValueSql("'SYSTEM'::character varying");
            });

            modelBuilder.Entity<TbTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("Pk_TbTransaction");

                entity.Property(e => e.TransactionId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("'SYSTEM'::character varying");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedBy).HasDefaultValueSql("'SYSTEM'::character varying");

                entity.HasOne(d => d.Makanan)
                    .WithMany(p => p.TbTransaction)
                    .HasForeignKey(d => d.MakananId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TbMakanan");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbTransaction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TbUser");
            });

            modelBuilder.Entity<TbUser>(entity =>
            {
                entity.HasIndex(e => e.UserName)
                    .HasName("TbUser_UserName_key")
                    .IsUnique();

                entity.Property(e => e.UserId).UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.CreatedBy).HasDefaultValueSql("'SYSTEM'::character varying");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedBy).HasDefaultValueSql("'SYSTEM'::character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
