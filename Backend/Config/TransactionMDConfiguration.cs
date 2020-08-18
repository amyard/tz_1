using Backend.Enums;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Backend.Config
{
    public class TransactionMDConfiguration : IEntityTypeConfiguration<TransactionMD>
    {
        public void Configure(EntityTypeBuilder<TransactionMD> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.TransactionId).IsRequired();
            builder.Property(x => x.ClientName).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            builder.Property(x => x.Status).IsRequired()
                .HasConversion(
                    o => o.ToString(),
                    o => (TransactionStatus)Enum.Parse(typeof(TransactionStatus), o)
                );

            builder.Property(x => x.Type).IsRequired()
                .HasConversion(
                    o => o.ToString(),
                    o => (TransactionType)Enum.Parse(typeof(TransactionType), o)
                );
        }
    }
}
