
namespace SurveyBasket.Models
{
    public class Poll: AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; }   
        public DateOnly CreatedAt { get; set; }
        public DateOnly EndAt { get; set; } 


    }
}
