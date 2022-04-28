using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClassLibrary1.Entity
{
    public class Option
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public Question? Question { get; set; }

        // this relationship is configured as a required, since the Option.QuestionId foreign key property is non-nullable
        // https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#deleting-a-principalparent
        public int QuestionId { get; set; }

        [NotMapped]
        public int Count { get; set; }

        [NotMapped]
        public int Percent { get; set; }
    }
}