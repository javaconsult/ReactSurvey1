using ClassLibrary1.DTOs;

namespace WebApi.Services
{
    public interface IParticipantService
    {
        Task<CreateParticipantResponse> CreateAsync(ParticipantDto participantDto);
        Task<UploadSurveyResponse> UploadCompletedSurveyAsync(UploadSurveyDto uploadSurveyDto);
        Task<GetSurveyResponse> GetSurveyForParticipant(Guid participantId);
    }
}
