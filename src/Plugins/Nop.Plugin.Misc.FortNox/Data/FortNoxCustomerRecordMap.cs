using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Misc.FortNox.Domain;

namespace Nop.Plugin.Misc.FortNox.Data
{
    /// <summary>
    /// Represents a shipping by weight or by total record mapping configuration
    /// </summary>
    public partial class FortNoxCustomerRecordMap : NopEntityTypeConfiguration<FortNoxCustomerRecord>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<FortNoxCustomerRecord> builder)
        {
            builder.ToTable(nameof(FortNoxCustomerRecord));
            builder.HasKey(record => record.Id);
            builder.Property(record => record.CustomerNumber);
            builder.Property(record => record.CustomerNumberNox);
            builder.Property(record => record.LastSyncDate);
            builder.Property(record => record.LastSyncedBy);
            builder.Property(record => record.OrganisationNumber);
            builder.Property(record => record.Type);

        }

        #endregion
    }
}