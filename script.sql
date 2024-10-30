-- Create the database
CREATE DATABASE FStore;
GO

USE FStore;
GO

CREATE TABLE [Role] (
	RoleId INT PRIMARY KEY IDENTITY(1,1),
	RoleName VARCHAR(100) NOT NULL,
);
GO

CREATE TABLE [Member] (
    [MemberId] INT PRIMARY KEY IDENTITY(1,1),
    [Email] VARCHAR(100) NOT NULL,
    [CompanyName] VARCHAR(40) NOT NULL,
    [City] VARCHAR(15) NOT NULL,
    [Country] VARCHAR(15) NOT NULL,
    [Password] VARCHAR(MAX) NOT NULL,
	[RoleId] INT NOT NULL,
	FOREIGN KEY (RoleId) REFERENCES Role(RoleId)
);
GO

CREATE TABLE BookCategory (
	CategoryId INT PRIMARY KEY IDENTITY(1, 1),
	[Name] VARCHAR(100) NOT NULL,
);
GO

CREATE TABLE Book (
	BookId INT PRIMARY KEY IDENTITY(1,1),
    CategoryId INT NOT NULL,
    BookName NVARCHAR(100) NOT NULL,
	Author NVARCHAR(100) NOT NULL,
    Publisher NVARCHAR(100) NOT NULL,
	[Year] INT NOT NULL,
	[Description] NVARCHAR(255) NOT NULL,
    UnitPrice MONEY NOT NULL,
    UnitsInStock INT NOT NULL,
	ImageUrl NVARCHAR(1000) NOT NULL,
	FOREIGN KEY (CategoryId) REFERENCES BookCategory(CategoryId),
);
GO

-- Create the Order table
CREATE TABLE [Order] (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    MemberId INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    ShippedDate DATETIME NULL,
    TotalPrice MONEY NULL,
    FOREIGN KEY (MemberId) REFERENCES Member(MemberId)
);
GO

-- Create the OrderDetail table
CREATE TABLE OrderDetail (
    OrderId INT NOT NULL,
    BookId INT NOT NULL,
    UnitPrice MONEY NOT NULL,
    Quantity INT NOT NULL,
    Discount FLOAT NULL,
    PRIMARY KEY (OrderId, BookId),
    FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    FOREIGN KEY (BookId) REFERENCES Book(BookId)
);
GO

INSERT INTO BookCategory ([Name])
VALUES
('Fiction'),
('Non-Fiction'),
('Manga'),
('Fantasy'),
('Mystery'),
('Biography'),
('History'),
('Self-Help'),
('Travel'),
('Children');

INSERT INTO [Role] (RoleName) VALUES ('ADMIN')
INSERT INTO [Role] (RoleName) VALUES ('USER')

INSERT INTO [Member] (Email, Country, City, Password, RoleId)
VALUES ('phucnt40@fpt.edu.vn', 'China', 'Shanghai', '$2y$10$Xka0uqsEVjQWHk/EOClKwOcoNSYuSdt6WuTqV9cgBWfglBXXVC7U2', 1); //password: 123123
INSERT INTO [Member] (Email, Country, City, [CompanyName], [Password], RoleId)
VALUES ('ubro3@32mine.net', 'China', 'Shanghai', '32MINE', '$2y$10$Xka0uqsEVjQWHk/EOClKwOcoNSYuSdt6WuTqV9cgBWfglBXXVC7U2', 2); --password: 123123

INSERT INTO Book (CategoryId, BookName, Author, Publisher, [Year], [Description], UnitPrice, UnitsInStock, ImageUrl)
VALUES
(1, 'The Great Gatsby', 'F. Scott Fitzgerald', 'Scribner', 1925, 'A novel set in the Jazz Age that tells the story of Jay Gatsby and his unrequited love for Daisy Buchanan.', 10.99, 100, 'https://example.com/images/greatgatsby.jpg'),
(2, 'Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', 'Harper', 2011, 'A narrative history of humanity from the Stone Age to the modern age.', 18.99, 50, 'https://example.com/images/sapiens.jpg'),
(3, 'Dune', 'Frank Herbert', 'Chilton Books', 1965, 'A science fiction novel about the desert planet Arrakis and its people.', 15.99, 30, 'https://example.com/images/dune.jpg'),
(4, 'The Hobbit', 'J.R.R. Tolkien', 'George Allen & Unwin', 1937, 'A fantasy novel about Bilbo Baggins and his adventure to win a share of the treasure guarded by Smaug the dragon.', 12.99, 70, 'https://example.com/images/hobbit.jpg'),
(5, 'The Hound of the Baskervilles', 'Arthur Conan Doyle', 'George Newnes', 1902, 'A Sherlock Holmes mystery involving a family curse and a terrifying hound.', 9.99, 60, 'https://example.com/images/baskervilles.jpg'),
(6, 'The Diary of a Young Girl', 'Anne Frank', 'Contact Publishing', 1947, 'The wartime diary of Anne Frank, a young Jewish girl hiding from the Nazis.', 8.99, 80, 'https://example.com/images/annefrank.jpg'),
(7, 'The History of the Ancient World', 'Susan Wise Bauer', 'W.W. Norton & Company', 2007, 'A detailed account of ancient civilizations from Mesopotamia to the fall of Rome.', 20.99, 40, 'https://example.com/images/ancientworld.jpg'),
(8, 'Atomic Habits', 'James Clear', 'Avery', 2018, 'A guide on how small habits can lead to big changes in life.', 16.99, 90, 'https://example.com/images/atomichabits.jpg'),
(9, 'Lonely Planet: Japan', 'Lonely Planet', 'Lonely Planet', 2020, 'A travel guide to Japan, covering culture, landmarks, and local tips.', 24.99, 20, 'https://example.com/images/japan.jpg'),
(10, 'Harry Potter and the Sorcerer''s Stone', 'J.K. Rowling', 'Bloomsbury', 1997, 'The first book in the Harry Potter series, where Harry learns about his magical heritage.', 10.99, 120, 'https://example.com/images/harrypotter.jpg');