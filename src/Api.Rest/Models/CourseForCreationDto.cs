using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Rest.Models
{
    public class CourseForCreationDto : CourseForManipulationDto, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (Title == Description)
            {
                yield return new ValidationResult("The provided description should be different from title", new[] { "CourseForCreationDto" });
            }
        }
    }
}