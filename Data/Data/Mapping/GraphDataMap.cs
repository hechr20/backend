using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.DbEntities;

namespace Data.Mapping
{
    public class GraphDataMap : MappingEntityTypeConfiguration<GraphData>
    {
        public override void Configure(EntityTypeBuilder<GraphData> builder)
        {
            builder.ToTable("GraphData");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.UTCTimestamp).HasColumnType("DateTime").HasDefaultValueSql("GetUtcDate()");
            base.Configure(builder);
        }
    }
}
