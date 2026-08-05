CREATE DATABASE FinancialTracker;
GO
USE FinancialTracker;

CREATE TABLE Users (
	userId INT IDENTITY(1,1) PRIMARY KEY,
	email nvarchar(255) NULL,
	[password] [nvarchar] (255) NULL,
	creationTime date NULL
);

CREATE TABLE Transactions (
	transactionsID INT IDENTITY(1,1) PRIMARY KEY,
	amount int NOT NULL,
	[date] [date] NOT NULL,
	userID int NOT NULL,
	categoryID int NULL
);

CREATE TABLE Categories (
	categoryID INT IDENTITY(1,1) PRIMARY KEY,
	userID int NOT NULL,
	[name] [nvarchar] (100) NULL,
	[type] [nvarchar] (100) NULL
);

CREATE TABLE Budget (
	budgetID INT IDENTITY(1,1) PRIMARY KEY,
	limits int NULL,
	[date] [date] NOT NULL,
	userID int NULL,
	categoryID int NULL
);

ALTER TABLE Users
ADD CONSTRAINT UQ_Users_Email UNIQUE (email);

ALTER TABLE Budget
ADD CONSTRAINT FK_Budget_Category
FOREIGN KEY (categoryID)
REFERENCES Categories(categoryID);

ALTER TABLE Transactions
ADD CONSTRAINT FK_Transactions_Category
FOREIGN KEY (categoryID)
REFERENCES Categories(categoryID);

ALTER TABLE Budget
ADD CONSTRAINT FK_Budget_User
FOREIGN KEY (userID)
REFERENCES Users(userID);