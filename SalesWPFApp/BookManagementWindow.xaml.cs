using BusinessObject.Entities;
using Microsoft.Win32;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for BookManagementWindow.xaml
    /// </summary>
    public partial class BookManagementWindow : Window
    {
        private IBookRepository _bookRepository;
        private IBookCategoryRepository _bookcategoryRepository;

        public BookManagementWindow()
        {
            InitializeComponent();
            _bookRepository = new BookRepository();
            _bookcategoryRepository = new BookCategoryRepository();
        }

        private void LoadCategories()
        {
            // Sample list of categories, replace with your actual data source
            var categories = _bookcategoryRepository.GetAllCategories();

            // Bind categories to ComboBox
            cbbCategory.ItemsSource = categories;
        }

        private void LoadBooks()
        {
            try
            {
                var bookList = _bookRepository.GetAllBooks();
                BookDataGrid.ItemsSource = bookList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearInputFields()
        {
            // Clear TextBox inputs
            txtBookName.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtPublisher.Text = string.Empty;
            txtYear.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtUnitsInStock.Text = string.Empty;
            txtImageUrl.Text = string.Empty;

            // Reset ComboBox selection
            cbbCategory.SelectedIndex = -1; // Clear the selection
        }

        private void BookDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bookList = _bookRepository.GetAllBooks();
                BookDataGrid.ItemsSource = bookList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookDataGrid.SelectedItem is Book selectedBook)
            {
                txtBookName.Text = selectedBook.BookName;
                txtAuthor.Text = selectedBook.Author;
                txtPublisher.Text = selectedBook.Publisher;
                txtYear.Text = selectedBook.Year.ToString();
                txtDescription.Text = selectedBook.Description;
                txtUnitPrice.Text = selectedBook.UnitPrice.ToString();
                txtUnitsInStock.Text = selectedBook.UnitsInStock.ToString();
                txtImageUrl.Text = selectedBook.ImageUrl;
                cbbCategory.SelectedValue = selectedBook.CategoryId;
            }
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                string filePath = openFileDialog.FileName;
                MessageBox.Show(filePath);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBooks();
            LoadCategories();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchBook = SearchTextBox.Text.ToLower();
            var allBooks = _bookRepository.GetAllBooks();
            var motorbikes = allBooks.Where(m =>
            m.BookName.ToLower().Contains(searchBook) ||
            m.Author.ToLower().Contains(searchBook) ||
            m.Description.ToLower().Contains(searchBook) ||
            m.Publisher.ToLower().Contains(searchBook)).ToList();
            BookDataGrid.ItemsSource = motorbikes;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchButton_Click(sender, e);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Input validation
                if (string.IsNullOrWhiteSpace(txtBookName.Text))
                {
                    MessageBox.Show("Book Name is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAuthor.Text))
                {
                    MessageBox.Show("Author is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPublisher.Text))
                {
                    MessageBox.Show("Publisher is required.");
                    return;
                }

                if (!int.TryParse(txtYear.Text, out int year))
                {
                    MessageBox.Show("Invalid year. Please enter a valid number.");
                    return;
                }

                if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice))
                {
                    MessageBox.Show("Invalid unit price. Please enter a valid number.");
                    return;
                }

                if (!int.TryParse(txtUnitsInStock.Text, out int unitsInStock))
                {
                    MessageBox.Show("Invalid units in stock. Please enter a valid number.");
                    return;
                }

                if (cbbCategory.SelectedValue == null)
                {
                    MessageBox.Show("Please select a category.");
                    return;
                }

                // Creating a new Book object
                Book book = new Book()
                {
                    BookName = txtBookName.Text,
                    Author = txtAuthor.Text,
                    Publisher = txtPublisher.Text,
                    Year = year,
                    Description = txtDescription.Text,
                    UnitPrice = unitPrice,
                    UnitsInStock = unitsInStock,
                    ImageUrl = txtImageUrl.Text, 
                    CategoryId = (int)cbbCategory.SelectedValue
                };

                // Add the book to the repository
                _bookRepository.AddBook(book);

                MessageBox.Show("Book added successfully!");

                // Reload books and clear the input fields
                LoadBooks();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a book is selected in the DataGrid
                if (BookDataGrid.SelectedItem is Book selectedBook)
                {
                    // Validate input fields
                    if (string.IsNullOrWhiteSpace(txtBookName.Text) ||
                        string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                        string.IsNullOrWhiteSpace(txtPublisher.Text) ||
                        !int.TryParse(txtYear.Text, out int year) ||
                        string.IsNullOrWhiteSpace(txtDescription.Text) ||
                        !decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) ||
                        !int.TryParse(txtUnitsInStock.Text, out int unitsInStock) ||
                        cbbCategory.SelectedValue == null)
                    {
                        MessageBox.Show("Please fill out all fields correctly.");
                        return;
                    }

                    // Update the selected book with new values
                    selectedBook.BookName = txtBookName.Text;
                    selectedBook.Author = txtAuthor.Text;
                    selectedBook.Publisher = txtPublisher.Text;
                    selectedBook.Year = year;
                    selectedBook.Description = txtDescription.Text;
                    selectedBook.UnitPrice = unitPrice;
                    selectedBook.UnitsInStock = unitsInStock;
                    selectedBook.ImageUrl = txtImageUrl.Text;
                    selectedBook.CategoryId = (int)cbbCategory.SelectedValue;

                    // Save changes in the repository
                    _bookRepository.UpdateBook(selectedBook);

                    MessageBox.Show("Book updated successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a book to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // Reload the book list and clear the input fields
                LoadBooks();
                ClearInputFields();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a book is selected in the DataGrid
                if (BookDataGrid.SelectedItem is Book selectedBook)
                {
                    // Confirm deletion
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedBook.BookName}'?",
                        "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Delete the book
                        _bookRepository.DeleteBook(selectedBook.BookId);
                        MessageBox.Show("Book deleted successfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a book to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // Reload the book list and clear the input fields
                LoadBooks();
                ClearInputFields();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();

            this.Hide();
        }
    }
}
