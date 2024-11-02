using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Categories
{
    [Authorize(Roles = "ADMIN")]
    public class CategoriesModel : PageModel
    {
        private readonly IBookCategoryService _bookRepository;

        public List<BookCategory> Categories { get; set; } = new List<BookCategory>();

        public CategoriesModel(IBookCategoryService bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void OnGet()
        {
            Categories = _bookRepository.GetAllCategories();
        }
    }
}
