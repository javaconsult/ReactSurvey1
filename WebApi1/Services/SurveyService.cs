using AutoMapper;
using ClassLibrary1.DTOs;
using ClassLibrary1.Entity;
using ClassLibrary1.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository surveyRepository;
        private readonly UserManager<AdminUser> userManager;
        private readonly IMapper mapper;

        public SurveyService(ISurveyRepository surveyRepository, UserManager<AdminUser> userManager, IMapper mapper)
        {
            this.surveyRepository = surveyRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<ICollection<SurveyListItemDto>> GetSurveysByUsername(string username)
        {
            string adminUserId = userManager.Users.First(u => u.UserName == username).Id;
            ICollection<Survey> surveys = await surveyRepository.SelectByAdminUserIdAsync(adminUserId);
            ICollection<SurveyListItemDto> surveyDtos = mapper.Map<ICollection<SurveyListItemDto>>(surveys);
            return surveyDtos;
        }
        public async Task<Guid> CreateSurvey(SurveyDto surveyDto, string userName)
        {
            Survey survey = mapper.Map<Survey>(surveyDto);
            string adminUserId = userManager.Users.First(adminUser => adminUser.UserName == userName).Id;
            survey.AdminUserId = adminUserId; //associate survey with authenticated user
            Guid surveyId = await surveyRepository.CreateAsync(survey);
            return surveyId;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await surveyRepository.DeleteAsync(id);
        }

        public async Task<SurveyDto> SelectResultsBySurveyIdAsync(Guid id)
        {
            Survey? survey = await surveyRepository!.SelectByIdAsync(id);
            if (survey == null)
                throw new SurveyNotFoundException();

            //null survey date indicates a participant who has been created but has not completed the survey
            //remove these participants from the survey results
            survey.Participants = survey.Participants.Where(p => p.SurveyDate != null).ToList();

            foreach (Participant participant in survey.Participants)
            {
                if (participant.Selections == null) break;
                foreach (Selection selection in participant.Selections)
                {
                    //for each question answered by a participant, increment the count for the selected option
                    Option option = survey.Questions.First(q => q.Id == selection.QuestionId).Options.First(o => o.Id == selection.SelectedOptionId);
                    option.Count++;

                    option.Percent = Convert.ToInt32(Math.Round(option.Count * 100.0 / survey.Participants.Count));
                }
            }

            SurveyDto surveyDto = mapper.Map<SurveyDto>(survey);
            surveyDto.ParticipantCount = survey.Participants.Count;
            return surveyDto;
        }

        public async Task<ICollection<ParticipantDto>> SelectParticipantsBySurveyId(Guid surveyId)
        {
            ICollection<Participant> participants = await surveyRepository.SelectParticipantsBySurveyId(surveyId);
            ICollection<ParticipantDto> participantDtos = mapper.Map<ICollection<ParticipantDto>>(participants);
            return participantDtos;
        }

        public async Task<ICollection<SelectedOptionDto>> SelectionsByParticipantId(Guid participantId)
        {
            Participant? participant = await surveyRepository.SelectParticipantById(participantId);
            List<Question> questions = await surveyRepository.SelectQuestionsBySurveyId(participant!.SurveyId);

            List<SelectedOptionDto> selectedOptionDtos = new List<SelectedOptionDto>();
            if (participant.Selections != null)
                foreach (Selection selection in participant.Selections)
                {
                    Question question = questions.First(q => q.Id == selection.QuestionId);
                    Option option = question.Options.First(o => o.Id == selection.SelectedOptionId);
                    selectedOptionDtos.Add(new SelectedOptionDto(question.Text, option.Text));
                }
            return selectedOptionDtos;
        }
    }
}
