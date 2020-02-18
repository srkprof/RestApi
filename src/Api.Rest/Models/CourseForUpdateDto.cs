using System.ComponentModel.DataAnnotations;

namespace Api.Rest.Models
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage = "This is required.")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}