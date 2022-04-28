using AutoMapper;
using ClassLibrary1.Entity;
using ClassLibrary1.DTOs;

namespace ClassLibrary1.DTOs
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //source, destination
            CreateMap<UploadSurveyDto, Participant>(); //ParticipantController.Update
            CreateMap<SelectionDto, Selection>(); //UploadSurveyDto

            //https://docs.automapper.org/en/latest/Reverse-Mapping-and-Unflattening.html
            CreateMap<ParticipantDto, Participant>().ReverseMap(); //ParticipantController.Create, SurveyAdminService.SelectParticipantsBySurveyId

            CreateMap<Survey, SurveyListItemDto>(); // SurveyController.GetSurveysForLoggedInUser
            CreateMap<Survey, SurveyDto>().ReverseMap(); //ParticipantController.Index,  SurveyController.GetResultsBySurveyIdAsync, SurveyController.CreateSurvey
            CreateMap<Question, QuestionDto>().ReverseMap();//SurveyController.CreateSurvey
            CreateMap<Option, OptionDto>().ReverseMap();//SurveyController.CreateSurvey

            //map AdminUser to UserDto
            CreateMap<AdminUser, UserDto>();
            //map RegisterDto to AdminUser
            CreateMap<RegisterDto, AdminUser>();
        }
    }
}
