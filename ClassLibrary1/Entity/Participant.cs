using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Entity
{
    public class Participant
    {
        //public Participant()
        //{

        //}
        //public Participant(Guid surveyId)
        //{
        //    SurveyId = surveyId;
        //}

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? SurveyDate { get; set; } = null;

        //Required relationships are configured to use cascade deletes by default.
        //https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete
        public ICollection<Selection>? Selections { get; set; }
        public Survey? Survey { get; set; }
        public Guid SurveyId { get; set; }
    }
}