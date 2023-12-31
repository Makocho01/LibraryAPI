﻿using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/Books")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BooksController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _dbContext.Books
                .Include(x => x.Genres)
                .Include(x => x.Authors)
                .Include(x => x.PublishingHouse)
                .ToList();

            return Ok(books);
        }

        // get id book
        [HttpGet("{id}")]
        public IActionResult GetBook([FromRoute] Guid id)
        {
            var book = _dbContext.Books
                .Include(x => x.Genres)
                .Include(x => x.Authors)
                .Include(x => x.PublishingHouse)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }


        [HttpPost]
        public IActionResult CreateBook(BookDto book) 
        {
            var genres = _dbContext.Genres.Where(g => book.Genres.Contains(g.Id)).ToList();
            var authors = _dbContext.Authors.Where(a => book.Authors.Contains(a.Id)).ToList();
            var publishingHouse = _dbContext.PublishingHouses.FirstOrDefault(p => p.Id == book.PublishingHouse);

            var newBook = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                RelaseDate = book.RelaseDate,
                ISBN = book.ISBN,
                Genres = genres,
                Authors = authors,
                PublishingHouse = publishingHouse
            };

            _dbContext.Books.Add(newBook);
            _dbContext.SaveChanges();
            
            return Ok(_dbContext.Books.FirstOrDefault(b => b.ISBN == book.ISBN)); 
        }

        [HttpPut("{id}")] //put = edit
        public IActionResult EditBook([FromRoute] Guid id, BookDto book)
        {
            var originalBook = _dbContext.Books.FirstOrDefault(r => r.Id == id);
            if (originalBook == null)
            {
                return NotFound();
            }

            var genres = _dbContext.Genres.Where(g => book.Genres.Contains(g.Id)).ToList();
            var authors = _dbContext.Authors.Where(a => book.Authors.Contains(a.Id)).ToList();

            var publishingHouse = _dbContext.PublishingHouses.FirstOrDefault(p => p.Id == book.PublishingHouse);
            if (publishingHouse == null)
            {
                return NotFound();
            }

            originalBook.Title = book.Title;
            originalBook.Description = book.Description;
            originalBook.RelaseDate = book.RelaseDate;
            originalBook.ISBN = book.ISBN; //co to do chuja jest ISBN
            originalBook.Genres = genres;
            originalBook.Authors = authors;
            originalBook.PublishingHouse = publishingHouse;

            _dbContext.SaveChanges();
            return Ok(_dbContext.Books.FirstOrDefault(r => r.Id == id));
        }




        // delete id author
        [HttpDelete("{id}")]
        public IActionResult DeleteBook([FromRoute] Guid id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
