namespace PromoWeb.Context.Entities
{
    public class Question : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
            
        public string? Email { get; set; }

        public virtual Answer? Answer { get; set; }

        public string RecipientEmail { get; set; }
    }
}
