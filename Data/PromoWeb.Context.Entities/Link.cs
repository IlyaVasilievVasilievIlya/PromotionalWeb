namespace PromoWeb.Context.Entities
{
    public class Link : BaseEntity
    {
        public string LinkText { get; set; }
        public string Description { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}
