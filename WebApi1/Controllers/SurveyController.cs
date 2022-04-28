using ClassLibrary1.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService) => this.surveyService = surveyService;
        
        public string AuthenticatedUsername => User.FindFirstValue(ClaimTypes.Name);

        [HttpGet]
        public async Task<ActionResult<ICollection<SurveyListItemDto>>> GetSurveysForLoggedInUser()
        {
            ICollection<SurveyListItemDto> surveyDtos = await surveyService.GetSurveysByUsername(AuthenticatedUsername);
            return Ok(surveyDtos);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSurveyAsync(SurveyDto surveyDto)
        {
            Guid surveyId = await surveyService.CreateSurvey(surveyDto, AuthenticatedUsername);
            return Ok(surveyId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurvey(Guid id)
        {
            bool deleted = await surveyService.DeleteAsync(id);
            return deleted ? Ok() : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDto>> GetResultsBySurveyIdAsync(Guid id)
        {
            SurveyDto surveyDto = await surveyService.SelectResultsBySurveyIdAsync(id);
            return surveyDto == null ? NotFound($"Survey {id} not found") : Ok(surveyDto);
        }

        [HttpGet("participants/{surveyId}")]
        public async Task<ActionResult<ICollection<ParticipantDto>>> GetParticipantsBySurvey(Guid surveyId)
        {
            var participants = await surveyService.SelectParticipantsBySurveyId(surveyId);
            return Ok(participants);
        }

        [HttpGet("selections/{participantId}")]
        public async Task<ActionResult<ICollection<SelectedOptionDto>>> GetSelectedOptionsByParticipant(Guid participantId)
        {
            ICollection<SelectedOptionDto> selectedOptions = await surveyService.SelectionsByParticipantId(participantId);
            return selectedOptions.Count == 0 ? NotFound() :  Ok(selectedOptions);
        }

    }
}