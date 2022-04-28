using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTOs
{
    public class UploadSurveyDto
    {
        [Required]
        public Guid Id { get; set; } //Participant Id
        public ICollection<SelectionDto>? Selections { get; set; }
    }
}
