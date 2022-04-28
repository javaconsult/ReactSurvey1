using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTOs
{
    public class SelectionDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        public int QuestionId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        public int SelectedOptionId { get; set; }
    }
}
