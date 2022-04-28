using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Entity
{
    public class Selection
    {
        public Selection()
        {

        }
        public Selection(int questionId, int selectedOptionId, Guid participantId)
        {
            QuestionId = questionId;
            SelectedOptionId = selectedOptionId;
            ParticipantId = participantId;
        }

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
        public Participant? Participant { get; set; }

        // this relationship is configured as a required, since the Selection.ParticipantId foreign key property is non-nullable
        // https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#deleting-a-principalparent
        public Guid ParticipantId { get; set; }
    }
}