using System;
using System.Data.Entity.ModelConfiguration;

namespace SGD.Dominio.Entidade.Video
{
    public class VideosYT
    {
        // Keys
        public int Id { get; set; }

        // Properties
        public string Nome { get; set; }
        public string Url { get; set; }
        public string LogoUrl { get; set; }
        public string CapaUrl { get; set; }
        public string Categoria { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataModificacao { get; set; }

    }

    public class VideoConfig : EntityTypeConfiguration<VideosYT>
    {
        public VideoConfig()
        {
            ToTable("Video");

            HasKey(u => u.Id);

            Property(u => u.Nome)
                .IsMaxLength()
                .HasColumnType("nvarchar(max)");

            Property(u => u.LogoUrl)
                .IsMaxLength()
                .HasColumnType("nvarchar(max)");

            Property(u => u.CapaUrl)
                .IsMaxLength()
                .HasColumnType("nvarchar(max)");

            Property(u => u.Url)
                .IsMaxLength()
                .HasColumnType("nvarchar(max)");

            Property(u => u.Categoria)
                .IsMaxLength()
                .HasColumnType("nvarchar(max)");
        }
    }
}