using Api.Rest.Entities;
using Api.Rest.Models;
using Api.Rest.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Api.Rest.Controllers
{
    [ApiController, Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICourseLibraryRepository courseLibraryRepository;

        public CoursesController(IMapper mapper, ICourseLibraryRepository courseLibraryRepository)
        {
            this.mapper = mapper;
            this.courseLibraryRepository = courseLibraryRepository;
        }

        [HttpGet]
        public ActionResult<CourseDto> GetCoursesForAuthor(Guid authorId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
                return NotFound();
            var courses = courseLibraryRepository.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("{courseId}", Name = "GetCourse")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
                return NotFound();
            var course = courseLibraryRepository.GetCourse(authorId, courseId);
            if (course == null)
                return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public IActionResult AddCourseForAuthor(Guid authorId, CourseForCreationDto courseDto)
        {
            var entity = mapper.Map<Course>(courseDto);
            courseLibraryRepository.AddCourse(authorId, entity);
            courseLibraryRepository.Save();
            return CreatedAtRoute("GetCourse", new { authorId = authorId, courseId = entity.Id }, entity);
        }
        [HttpPut("{courseId}")]
        public ActionResult UpdateCoursesForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto updateCourseDto)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
                return NotFound();
            var course = courseLibraryRepository.GetCourse(authorId, courseId);
            if (course == null)
            {
                var courseToAdd = mapper.Map<Course>(updateCourseDto);
                courseToAdd.Id = courseId;
                courseLibraryRepository.AddCourse(authorId, courseToAdd);
                courseLibraryRepository.Save();
                var courseToReturn = mapper.Map<CourseDto>(courseToAdd);
                return CreatedAtRoute("GetCourse", new { authorId, courseId = courseId, courseToReturn });
            }

            mapper.Map(course, updateCourseDto);
            courseLibraryRepository.UpdateCourse(course);
            courseLibraryRepository.Save();
            return NoContent();
        }
        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
                return NotFound();
            var course = courseLibraryRepository.GetCourse(authorId, courseId);

            if (course == null)
                return NotFound();
            //doing the reverse mapping to get the patch to apply
            var courseToPatch = mapper.Map<CourseForUpdateDto>(course);
            patchDocument.ApplyTo(courseToPatch, ModelState);
            if (!TryValidateModel(courseToPatch))
                return ValidationProblem(ModelState);
            mapper.Map(course, courseToPatch);
            courseLibraryRepository.Save();
            return NoContent();
        }
    }
}