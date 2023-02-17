namespace PromoWeb.Context.Entities
{
    public class Section : BaseEntity
    {
        public string SectionName { get; set; }
        public virtual ICollection<AppInfo> AppInfos { get; set; }
        public virtual ICollection<Link> Links { get; set; }
    }
}
