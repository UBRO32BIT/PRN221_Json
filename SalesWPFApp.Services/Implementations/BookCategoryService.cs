using BusinessObject.Entities;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
using SalesWPFApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Services.Implementations
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository _bookCategoryRepository;

        public BookCategoryService()
        {
            _bookCategoryRepository = new BookCategoryRepository();
        }
        public void AddCategory(BookCategory category)
            => _bookCategoryRepository.AddCategory(category);

        public BookCategory GetCategoryById(int id)
            => _bookCategoryRepository.GetCategoryById(id);

        public List<BookCategory> GetAllCategories()
            => _bookCategoryRepository.GetAllCategories();

        public void UpdateCategory(BookCategory category)
            => _bookCategoryRepository.UpdateCategory(category);

        public void DeleteCategory(int id)
            => _bookCategoryRepository.DeleteCategory(id);
    }
}
