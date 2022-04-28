using System;
using System.Collections.Generic;

namespace ClassLibrary1.DTOs
{
    public class SurveyDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<QuestionDto>? Questions { get; set; }
        public int ParticipantCount { get; set; }
        public Guid? ParticipantId { get; set; }
    }
}
