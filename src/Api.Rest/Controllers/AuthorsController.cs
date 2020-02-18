using Api.Rest.Entities;
using Api.Rest.Models;
using Api.Rest.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Api.Rest.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseRepository;
        private readonly IMapper mapper;

        public AuthorsController(ICourseLibraryRepository courseRepository, IMapper mapper)
        {
            this.courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //You can use a class instead of using parameters.
        [HttpGet]
        public IActionResult GetAuthors([FromQuery]string mainCategory, string searchQuery)
        {
            var authors = courseRepository.GetAuthors(mainCategory, searchQuery);

            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var result = courseRepository.GetAuthor(authorId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody]CreateAuthorDto createAuthorDto)
        {
            var author = mapper.Map<Author>(createAuthorDto);
            courseRepository.AddAuthor(author);
            courseRepository.Save();
            return CreatedAtRoute("GetAuthor", new { authorId = author.Id }, author);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }
    }
}