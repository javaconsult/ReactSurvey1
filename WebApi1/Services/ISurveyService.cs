using ClassLibrary1.DTOs;
using ClassLibrary1.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface ISurveyService
    {
        Task<ICollection<SurveyListItemDto>> GetSurveysByUsername(string username);
        Task<Guid> CreateSurvey(SurveyDto surveyDto, string username);
        Task<bool> DeleteAsync(Guid id);
        Task<SurveyDto> SelectResultsBySurveyIdAsync(Guid id);
        Task<ICollection<ParticipantDto>> SelectParticipantsBySurveyId(Guid surveyId);
        Task<ICollection<SelectedOptionDto>> SelectionsByParticipantId(Guid participantId);
    }
}
