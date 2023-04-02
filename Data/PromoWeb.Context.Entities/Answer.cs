namespace PromoWeb.Context.Entities
{
    public class Answer : BaseEntity
    {
        public DateTime Date { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public string Text { get; set; }
    }
}
