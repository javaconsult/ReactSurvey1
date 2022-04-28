
namespace ClassLibrary1.Entity
{
    public class Question
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; } = 1;
        public string Context { get; set; } = "";
        public string Heading { get; set; } = "";
        public string Text { get; set; } = "";
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public Survey? Survey { get; set; }

        // this relationship is configured as a required, since the Question.SurveyId foreign key property is non-nullable
        // https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#deleting-a-principalparent
        public Guid SurveyId { get; set; }      

        public override string ToString()
        {
            return Id + "  " + Text;
        }
    }
}