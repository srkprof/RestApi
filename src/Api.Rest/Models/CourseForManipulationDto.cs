using System.ComponentModel.DataAnnotations;

namespace Api.Rest.Models
{
    public abstract class CourseForManipulationDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The length of the string should not exceed 1500 characters")]
        public virtual string Description { get; set; }
    }
}