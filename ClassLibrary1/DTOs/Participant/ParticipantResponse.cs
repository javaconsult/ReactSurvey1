using ClassLibrary1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTOs
{
    public enum UpdateStatus
    {
        NotFound, //0
        NotUpdated, //1
        Conflict, //2
        Confirm //3
    }
    public class ParticipantResponse
    {
        public UpdateStatus Status { get; set; }
        public string StatusText => Status.ToString();
    }
    public class CreateParticipantResponse : ParticipantResponse
    {
        public Guid ParticipantId { get; set; }

    }
    public class UploadSurveyResponse : ParticipantResponse
    {
        public DateTime? SurveyDate { get; set; }
    }
    public class GetSurveyResponse : UploadSurveyResponse
    {
        public SurveyDto? SurveyDto { get; set; }
    }
}
