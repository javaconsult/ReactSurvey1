using System;
using ClassLibrary1.Entity;
using System.Threading.Tasks;

namespace ClassLibrary1.Repository
{
    public interface IParticipantRepository
    {
        Task<Guid> CreateAsync(Participant participant);
        Task<Participant?> FindParticipant(Guid participantId);
        Task<Survey?> SelectSurveyForParticipant(Guid participantId);
        Task<bool> UpdateAsync(Participant participant);
        Task<bool> SurveyExists(Guid surveyId);

        /// <returns>Participant including selections with matching email and survey id</returns>
        Task<Participant?> Select(string? participantEmail, Guid surveyId);
    }
}