using Api.Rest.DbContexts;
using Api.Rest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Rest.Repositories
{
    public class CourseLibraryRepository : ICourseLibraryRepository, IDisposable
    {
        private readonly CourseLibraryContext context;

        public CourseLibraryRepository(CourseLibraryContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));
            author.Id = new Guid();
            foreach (var course in author.Courses)
            {
                course.Id = new Guid();
            }
            context.Authors.Add(author);
        }

        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            course.AuthorId = authorId;
            context.Courses.Add(course);
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));
            return context.Authors.Any(x => x.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));
            context.Authors.Remove(author);
        }

        public void DeleteCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            context.Courses.Remove(course);
        }

        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));
            return context.Authors.FirstOrDefault(x => x.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return context.Authors.ToList();
        }

        public IEnumerable<Author> GetAuthors(string mainCategory, string searchQuery)
        {
            var authors = context.Authors.AsQueryable();

            if (!string.IsNullOrEmpty(mainCategory))
            {
                mainCategory = mainCategory.Trim();
                authors = authors.Where(x => x.MainCategory == mainCategory);
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                authors = authors.Where(x => x.MainCategory.Contains(searchQuery) || x.FirstName.Contains(searchQuery)
                || x.LastName.Contains(searchQuery));
            }
            return authors.ToList();
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            return context.Authors.Where(x => authorIds.Contains(x.Id)).OrderBy(a => a.FirstName).OrderBy(a => a.LastName).ToList();
        }

        public Course GetCourse(Guid authorId, Guid courseId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));
            if (courseId == Guid.Empty)
                throw new ArgumentNullException(nameof(courseId));
            return context.Courses.FirstOrDefault(x => x.AuthorId == authorId && x.Id == courseId);
        }

        public IEnumerable<Course> GetCourses(Guid authorId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));
            return context.Courses.Where(c => c.AuthorId == authorId).ToList();
        }

        public void UpdateAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void UpdateCourse(Course course)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return context.SaveChanges() >= 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}