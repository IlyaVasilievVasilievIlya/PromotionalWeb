namespace PromoWeb.Web
{
    public class QuestionListItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
