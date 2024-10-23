using BusinessObject.Entities;
using SalesWPFApp.DAOs;
using SalesWPFApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Repositories.Implementations
{
    public class BookCategoryRepository : IBookCategoryRepository
    {
        public void AddCategory(BookCategory category)
            => BookCategoryDAO.Instance.AddBookCategory(category);

        public BookCategory GetCategoryById(int id)
            => BookCategoryDAO.Instance.GetBookCategoryById(id);

        public List<BookCategory> GetAllCategories()
            => BookCategoryDAO.Instance.GetAllCategories();

        public void UpdateCategory(BookCategory category)
            => BookCategoryDAO.Instance.UpdateBookCategory(category);

        public void DeleteCategory(int id)
            => BookCategoryDAO.Instance.DeleteBookCategory(id);
    }
}
