using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using app2.Models;

namespace app2.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 设置表名
            builder.ToTable("users");

            // 设置主键
            builder.HasKey(e => e.id);

            // 配置字段属性
            builder.Property(e => e.id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd(); // 自增主键

            builder.Property(e => e.name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.create_time)
                .HasColumnName("create_time")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(e => e.update_time)
                .HasColumnName("update_time")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(e => e.is_delete)
                .HasColumnName("is_delete")
                .HasDefaultValue(false)
                .IsRequired();

            // 创建索引
            builder.HasIndex(e => e.name)
                .HasDatabaseName("idx_user_name");

            builder.HasIndex(e => e.is_delete)
                .HasDatabaseName("idx_user_is_delete");

            // 创建复合索引（用于软删除查询优化）
            builder.HasIndex(e => new { e.name, e.is_delete })
                .HasDatabaseName("idx_user_name_is_delete");
        }
    }
}