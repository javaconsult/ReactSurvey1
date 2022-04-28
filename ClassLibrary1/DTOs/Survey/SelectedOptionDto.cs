using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTOs
{
    public class SelectedOptionDto
    {
        public SelectedOptionDto(string? questionText, string? selectedOptionText)
        {
            QuestionText = questionText;
            OptionText = selectedOptionText;
        }

        public string? QuestionText { get; set; }
        public string? OptionText { get; set; }

    }
}
