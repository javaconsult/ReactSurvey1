using System.Runtime.Serialization;

namespace WebApi.Services
{
    [Serializable]
    public class SurveyNotFoundException : Exception
    {
        public SurveyNotFoundException()
        {
        }

        public SurveyNotFoundException(string? message) : base(message)
        {
        }
    }
}