namespace SurveyBasket.MappingConfig
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>()
             .Map(dest => dest.Answers, src => src.Answers.Select(Answer => new Answer { Content = Answer }));
        }
    }
}
