using ClassLibrary1.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantService participantService;

    public ParticipantController(IParticipantService participantService)
    {
        this.participantService = participantService;
    }

    /// <summary>
    /// Creates a Participant. NotFound response if survey doesn't exist. Conflict response if participant completed survey previously
    /// </summary>
    /// <returns>CreateParticipantResponse contains status enum and generated participant id</returns>
    [HttpPost] //api/participant
    public async Task<ActionResult<ParticipantResponse>> Create(ParticipantDto participantDto)
    {
        ParticipantResponse response = await participantService.CreateAsync(participantDto);
        switch (response.Status)
        {
            case UpdateStatus.Conflict:
                return Conflict();
            case UpdateStatus.NotFound:
                return NotFound();
        }
        return Ok(response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uploadSurveyDto">UploadSurveyDto contains participant id and collection of SelectionDto</param>
    /// <returns>
    /// 
    /// NotFound response if Participant not created. Conflict respaonse with survey date if previously uploaded
    /// </returns>    
    [HttpPut] //api/participant
    public async Task<ActionResult> UploadCompletedSurvey(UploadSurveyDto uploadSurveyDto)
    {
        UploadSurveyResponse response = await participantService.UploadCompletedSurveyAsync(uploadSurveyDto);
        switch (response.Status)
        {
            case UpdateStatus.NotFound:
                return NotFound($"This participant has not been registered");
            case UpdateStatus.Conflict:
                return Conflict(response.SurveyDate);
        }
        return response.Status == UpdateStatus.Confirm ? Ok() : BadRequest();
    }

    /// <summary>
    /// Get survey questions and options for a participant who has been previously created
    /// </summary>  
    /// <returns>
    /// 400 Bad Request if malformed GUID
    /// 404 Not Found participant id not found
    /// 409 Conflict if participant has already uploaded survey
    /// </returns>
    [HttpGet("{participantId}")] //api/participant/{participantId}
    public async Task<ActionResult<SurveyDto>> GetSurveyForParticipant(Guid participantId)
    {
        GetSurveyResponse response = await participantService.GetSurveyForParticipant(participantId);
        switch (response.Status)
        {
            case UpdateStatus.NotFound:
                return NotFound("Participant not found");
            case UpdateStatus.Conflict:
                return Conflict(response.SurveyDate);
        }
        return Ok(response.SurveyDto);
    }



}
