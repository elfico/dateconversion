using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DateTimeConversion.Models
{
    public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
    {
        public void Configure(EntityTypeBuilder<AccountTransaction> entity)
        {
            entity.HasKey(e => e.TransactionId);

            entity.Property(e => e.TransactionId).HasDefaultValueSql("(newid())");

            entity.Property(e => e.Amount).HasColumnType("decimal(19, 2)");

            entity.Property(e => e.Balance).HasColumnType("decimal(19, 2)");

            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.DateUpdated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.TransactionStatus).HasDefaultValueSql("((1))");
        }
    }
}