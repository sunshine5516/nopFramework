
using NopFramework.Core.Domains.Messages;
namespace NopFramework.Data.Mapping.Messages
{
    public class TermPictureMap: SunEntityTypeConfiguration<TermPicture>
    {
        public TermPictureMap()
        {
            this.ToTable("Term_Picture_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.Picture)
                .WithMany()
                .HasForeignKey(pp => pp.PictureId);


            this.HasRequired(pp => pp.Term)
                .WithMany(p => p.ProductPictures)
                .HasForeignKey(pp => pp.TermId);
        }
    }
}
