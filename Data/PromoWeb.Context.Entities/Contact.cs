namespace PromoWeb.Context.Entities
{
    public class Contact : BaseEntity
    {
        public string ContactOwner { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? WebSite { get; set; }
        public string? Phone { get; set; }
    }
}
