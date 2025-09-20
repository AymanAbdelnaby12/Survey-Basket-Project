namespace SurveyBasket.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; }   
        public DateTime CreatedAt { get; set; }
        public DateTime EndAt { get; set; } 


    }
}
