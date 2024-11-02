using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Books
{
    [Authorize(Roles = "ADMIN")]
    public class UpdateModel : PageModel
    {
        private readonly IBookService _bookRepository;
        private readonly IBookCategoryService _categoryRepository;

        [BindProperty]
        public Book book { get; set; }

        [BindProperty]
        public List<SelectListItem> categories { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }
        private readonly string _imagePath = Path.Combine("wwwroot", "Images");


        public UpdateModel(IBookService bookRepository, IBookCategoryService categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            book = new();
        }
        // Load the book data by ID
        public IActionResult OnGet(int id)
        {
            book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            // Populate category dropdown list
            categories = _categoryRepository.GetAllCategories()
                .Select(category => new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name
                })
                .ToList();

            return Page();
        }

        // Handle update form submission
        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid && !validateFile())
            {
                // Reload categories if there's a validation error
                categories = _categoryRepository.GetAllCategories()
                    .Select(category => new SelectListItem
                    {
                        Value = category.CategoryId.ToString(),
                        Text = category.Name
                    })
                    .ToList();
                return Page();
            }

            // Fetch the existing book from the database
            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // Update properties of the existing book
            existingBook.BookName = book.BookName;
            existingBook.Author = book.Author;
            existingBook.Publisher = book.Publisher;
            existingBook.Year = book.Year;
            existingBook.Description = book.Description;
            existingBook.UnitPrice = book.UnitPrice;
            existingBook.UnitsInStock = book.UnitsInStock;
            existingBook.CategoryId = book.CategoryId;

            // Handle file upload if ImageFile is not null
            if (ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
                var filePath = Path.Combine(_imagePath, fileName);

                // Ensure the Images directory exists
                Directory.CreateDirectory(_imagePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // Update the book's ImageUrl to the new image path
                existingBook.ImageUrl = $"/Images/{fileName}";
            }

            // Save the updated book to the database
            _bookRepository.UpdateBook(existingBook);

            return RedirectToPage("Index");
        }

        private bool validateFile()
        {
            return book != null && !(book.ImageUrl == null && ImageFile == null);
        }
    }
}
