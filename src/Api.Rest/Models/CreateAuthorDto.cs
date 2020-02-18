using System;
using System.Collections.Generic;

namespace Api.Rest.Models
{
    public class CreateAuthorDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string MainCategory { get; set; }
        public ICollection<CourseForCreationDto> Courses { get; set; } = new List<CourseForCreationDto>();
    }
}