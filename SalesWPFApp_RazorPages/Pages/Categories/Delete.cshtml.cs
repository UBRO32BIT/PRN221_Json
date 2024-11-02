using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Categories
{
    [Authorize(Roles = "ADMIN")]
    public class DeleteModel : PageModel
    {
        private readonly IBookCategoryService _bookCategoryRepository;
        private readonly IBookService _bookRepository;

        [BindProperty]
        public BookCategory Category { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public DeleteModel(IBookCategoryService bookCategoryRepository, IBookService bookRepository)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _bookRepository = bookRepository;
            Category = new();
        }

        public IActionResult OnGet(int id)
        {
            Category = _bookCategoryRepository.GetCategoryById(id);
            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            // Check if the category still has books assigned
            var booksInCategory = _bookRepository.GetBookByCategoryId(Category.CategoryId);
            if (booksInCategory.Any())
            {
                // Set an error message and return to the page without deleting
                ErrorMessage = "Cannot delete category. There are books assigned to this category.";
                return Page();
            }

            _bookCategoryRepository.DeleteCategory(Category.CategoryId);
            return RedirectToPage("./Index");
        }
    }
}