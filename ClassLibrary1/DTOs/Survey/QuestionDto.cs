using System.Collections.Generic;

namespace ClassLibrary1.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public int? QuestionNumber { get; set; } 
        public string? Heading { get; set; } 
        public string? Text { get; set; }
        public ICollection<OptionDto>? Options { get; set; } 
    }
}
