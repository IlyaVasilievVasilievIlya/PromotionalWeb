namespace PromoWeb.Context.Entities
{
    public class AppInfo : BaseEntity
    {
        public string TextTitle { get; set; }
        public string Text { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public virtual ICollection<Image> Screenshots { get; set; }
    }
}
