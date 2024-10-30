using BusinessObject.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using SalesWPFApp.Services.Implementations;
using SalesWPFApp.Services.Interfaces;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for BookManagementWindow.xaml
    /// </summary>
    public partial class BookManagementWindow : Window
    {
        private IBookService _bookService;
        private IBookCategoryService _bookCategoryService;

        public BookManagementWindow()
        {
            InitializeComponent();
            _bookService = new BookService();
            _bookCategoryService = new BookCategoryService();
        }

        private void LoadCategories()
        {
            try
            {
                // Sample list of categories, replace with your actual data source
                var categories = _bookCategoryService.GetAllCategories();

                // Bind categories to ComboBox
                cbbCategory.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBooks()
        {
            try
            {
                var bookList = _bookService.GetAllBooks();
                BookDataGrid.ItemsSource = bookList;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var bookList = _bookService.GetAllBooks();
                BookDataGrid.ItemsSource = bookList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };
                bool? response = openFileDialog.ShowDialog();

                if (response == true)
                {
                    string sourceFilePath = openFileDialog.FileName;

                    // Define the directory where images will be stored
                    string imagesDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    if (!System.IO.Directory.Exists(imagesDirectory))
                    {
                        System.IO.Directory.CreateDirectory(imagesDirectory);
                    }

                    // Generate a timestamp
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    // Get the original file name and add the timestamp as a prefix
                    string fileNameWithTimestamp = $"{timestamp}_{System.IO.Path.GetFileName(sourceFilePath)}";

                    // Set the destination file path with the timestamp-prefixed filename
                    string destinationFilePath = System.IO.Path.Combine(imagesDirectory, fileNameWithTimestamp);

                    // Copy the image to the destination path
                    System.IO.File.Copy(sourceFilePath, destinationFilePath, overwrite: true);

                    // Set the ImageUrl TextBox to the relative path
                    txtImageUrl.Text = destinationFilePath;

                    MessageBox.Show("Image saved and path set successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBooks();
            LoadCategories();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchBook = SearchTextBox.Text.ToLower();
                var allBooks = _bookService.GetAllBooks();
                var books = allBooks.Where(m =>
                m.BookName.ToLower().Contains(searchBook) ||
                m.Author.ToLower().Contains(searchBook) ||
                m.Description.ToLower().Contains(searchBook) ||
                m.Publisher.ToLower().Contains(searchBook)).ToList();
                BookDataGrid.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> validationFailedList = validateFields();
                if (!validationFailedList.IsNullOrEmpty())
                {
                    var messages = string.Join("\n", validationFailedList.Select(b => $"{b}"));
                    MessageBox.Show($"Validation failed: \n{messages}");
                    return;
                }

                decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice);
                int.TryParse(txtYear.Text, out int year);
                int.TryParse(txtUnitsInStock.Text, out int unitsInStock);
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

                _bookService.AddBook(book);

                MessageBox.Show("Book added successfully!");

                // Reload books and clear the input fields
                LoadBooks();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a book is selected in the DataGrid
                if (BookDataGrid.SelectedItem is Book selectedBook)
                {
                    List<string> validationFailedList = validateFields();
                    // Validate input fields
                    if (!validationFailedList.IsNullOrEmpty())
                    {
                        var messages = string.Join("\n", validationFailedList.Select(b => $"{b}"));
                        MessageBox.Show($"Validation failed: \n{messages}");
                        return;
                    }

                    decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice);
                    int.TryParse(txtYear.Text, out int year);
                    int.TryParse(txtUnitsInStock.Text, out int unitsInStock);

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
                    _bookService.UpdateBook(selectedBook);

                    MessageBox.Show("Book updated successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a book to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        _bookService.DeleteBook(selectedBook.BookId);
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
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown(); // Exit the application
        }

        private List<string> validateFields()
        {
            List<string> validationFailedList = new List<string>();
            // Input validation
            if (string.IsNullOrWhiteSpace(txtBookName.Text))
            {
                validationFailedList.Add("Book Name is required.");
            }
            else if (txtBookName.Text.Length > 100)
            {
                validationFailedList.Add("Book Name cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                validationFailedList.Add("Description is required.");
            }

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                validationFailedList.Add("Author is required.");
            }
            else if (txtAuthor.Text.Length > 100)
            {
                validationFailedList.Add("Book Name cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(txtPublisher.Text))
            {
                validationFailedList.Add("Publisher is required.");
            }

            if (!int.TryParse(txtYear.Text, out int year))
            {
                validationFailedList.Add("Invalid year. Please enter a valid number.");
            }
            else if (year < 0)
            {
                validationFailedList.Add("Invalid year. Please enter a valid number.");
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice))
            {
                validationFailedList.Add("Invalid unit price. Please enter a valid number.");
            }
            else if (unitPrice <= 0)
            {
                validationFailedList.Add("Unit price must be greater than 0.");
            }

            if (!int.TryParse(txtUnitsInStock.Text, out int unitsInStock))
            {
                validationFailedList.Add("Invalid units in stock. Please enter a valid number.");
            }
            else if (unitsInStock < 0)
            {
                validationFailedList.Add("Units in stock must not be a negative number.");
            }

            if (cbbCategory.SelectedValue == null)
            {
                validationFailedList.Add("Please select a category.");
            }

            if (string.IsNullOrWhiteSpace(txtImageUrl.Text))
            {
                validationFailedList.Add("Image is required");
            }
            return validationFailedList;
        }
    }
}
