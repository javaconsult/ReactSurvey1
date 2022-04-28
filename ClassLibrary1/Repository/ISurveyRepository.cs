using System;
using ClassLibrary1.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary1.DTOs;

namespace ClassLibrary1.Repository
{
    public interface ISurveyRepository
    {
        ICollection<Survey> SelectAll();
        Task<Guid> CreateAsync(Survey survey);
        Task<ICollection<Survey>> SelectByAdminUserIdAsync(string adminUserId);

        /// <returns>Survey with questions, options, participants and selections</returns>
        Task<Survey?> SelectByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<ICollection<Participant>> SelectParticipantsBySurveyId(Guid id);

        /// <returns>Participant including associated selections </returns>
        Task<Participant?> SelectParticipantById(Guid id);

        /// <returns>Questions including associated options</returns>
        Task<List<Question>> SelectQuestionsBySurveyId(Guid id);
    }
}