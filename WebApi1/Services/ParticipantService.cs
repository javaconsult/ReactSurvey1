using AutoMapper;
using ClassLibrary1.DTOs;

namespace WebApi.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository participantRepository;
        private readonly IMapper mapper;

        public ParticipantService(IParticipantRepository participantRepository, IMapper mapper)
        {
            this.participantRepository = participantRepository;
            this.mapper = mapper;
        }
        public async Task<CreateParticipantResponse> CreateAsync(ParticipantDto participantDto)
        {
            bool surveyExists = await participantRepository.SurveyExists(participantDto.SurveyId);
            if (!surveyExists)
                return new CreateParticipantResponse { Status = UpdateStatus.NotFound };

            Participant? previouslyCreatedParticipant = await participantRepository!.Select(participantDto.Email, participantDto.SurveyId);
            if (previouslyCreatedParticipant != null)
            {
                bool completed = previouslyCreatedParticipant.Selections!.Any();
                return completed ?
                    new CreateParticipantResponse { ParticipantId = previouslyCreatedParticipant.Id, Status = UpdateStatus.Conflict } :
                    new CreateParticipantResponse { ParticipantId = previouslyCreatedParticipant.Id, Status = UpdateStatus.NotUpdated };
            }

            Participant participant = mapper.Map<Participant>(participantDto);
            Guid participantId = await participantRepository.CreateAsync(participant);
            return new CreateParticipantResponse { ParticipantId = participantId, Status = UpdateStatus.Confirm };
        }

        public async Task<GetSurveyResponse> GetSurveyForParticipant(Guid participantId)
        {
            Participant? participant = await participantRepository.FindParticipant(participantId);
            if (participant == null)
            {
                return new GetSurveyResponse { Status = UpdateStatus.NotFound };
            }
            if (participant != null && participant.SurveyDate != null)
            {
                return new GetSurveyResponse { Status = UpdateStatus.Conflict, SurveyDate = participant.SurveyDate };
            }
            Survey? survey = await participantRepository.SelectSurveyForParticipant(participantId);
            SurveyDto surveyDto = mapper.Map<SurveyDto>(survey);
            surveyDto.ParticipantId = participant!.Id;
            return new GetSurveyResponse { Status= UpdateStatus.Confirm, SurveyDto = surveyDto };
        }

        public async Task<UploadSurveyResponse> UploadCompletedSurveyAsync(UploadSurveyDto uploadSurveyDto)
        {
            Participant? participant = await participantRepository.FindParticipant(uploadSurveyDto.Id);
            if (participant == null)
            {
                return new UploadSurveyResponse { Status = UpdateStatus.NotFound };
            }
            if (participant.SurveyDate != null)
            {
                return new UploadSurveyResponse { Status = UpdateStatus.Conflict, SurveyDate = participant.SurveyDate };
            }

            mapper.Map(uploadSurveyDto, participant);
            participant.SurveyDate = DateTime.Now;
            bool responseAccepted = await participantRepository.UpdateAsync(participant);
            return responseAccepted ?
                new UploadSurveyResponse { Status = UpdateStatus.Confirm } :
                new UploadSurveyResponse { Status = UpdateStatus.NotUpdated };
        }
    }
}
