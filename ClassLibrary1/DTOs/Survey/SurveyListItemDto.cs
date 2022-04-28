using System;
using System.Collections.Generic;

namespace ClassLibrary1.DTOs
{
    public class SurveyListItemDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
