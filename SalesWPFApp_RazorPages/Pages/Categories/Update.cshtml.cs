using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Categories
{
    [Authorize(Roles = "ADMIN")]
    public class UpdateModel : PageModel
    {
        private readonly IBookCategoryService _categoryRepository;

        [BindProperty]
        public BookCategory category { get; set; }

        public UpdateModel(IBookCategoryService categoryRepository)
        {
            _categoryRepository = categoryRepository;
            category = new();
        }
        // Load the book data by ID
        public IActionResult OnGet(int id)
        {
            category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Page();
        }

        // Handle update form submission
        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch the existing book from the database
            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;

            // Save the updated book to the database
            _categoryRepository.UpdateCategory(existingCategory);

            return RedirectToPage("Index");
        }
    }
}
