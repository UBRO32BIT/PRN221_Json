using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Books
{
    [Authorize(Roles = "ADMIN")]
    public class CreateModel : PageModel
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


        public CreateModel(IBookService bookRepository, IBookCategoryService categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            book = new();
        }

        public void OnGet()
        {
            categories = _categoryRepository.GetAllCategories()
                .Select(category => new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name
                })
                .ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid && ImageFile == null)
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
            var fileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
            var filePath = Path.Combine(_imagePath, fileName);
            // Ensure the Images directory exists
            Directory.CreateDirectory(_imagePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

            // Update book's ImageUrl with the relative path
            book.ImageUrl = $"/Images/{fileName}";


            _bookRepository.AddBook(book);
            return RedirectToPage("Index");
        }
    }
}
