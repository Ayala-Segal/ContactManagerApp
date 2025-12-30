CREATE TABLE People (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    ImagePath NVARCHAR(255) NULL
);

INSERT INTO People (FullName, Phone, Email, ImagePath)
VALUES 
('John Doe', '0501234567', 'john.doe@example.com', NULL),
('Jane Smith', '0529876543', 'jane.smith@example.com', NULL),
('Michael Brown', '0512345678', 'michael.brown@example.com', NULL),
('Emily Davis', '0537654321', 'emily.davis@example.com', NULL),
('David Wilson', '0541122334', 'david.wilson@example.com', NULL);

