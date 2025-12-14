# People Management Application

**Version:** 1.0
**Author:** Ayala Segal

## Description

A simple web application for managing people.
Allows adding new users, searching existing users, and exporting the list to PDF.

## Technologies

- C#  
- ASP.NET MVC 4.8  
- SQL Server (LocalDB or full server)  
- HTML, CSS, JavaScript  
- iTextSharp (for PDF export)

## Requirements

* Windows with Visual Studio 2019/2022
* SQL Server (LocalDB or full server) with the `People` table (see `CreatePeopleTable.sql`)
* ASP.NET MVC (Framework 4.8)

## Setup

1. Run the `CreatePeopleTable.sql` file in SQL Server.

   * This creates the `People` table and adds 5 example records.
2. Make sure the database connection is correct in `Web.config`.

   * Example:
     <connectionStrings>
       <add name="PeopleDb" connectionString="Server=(localdb)\MSSQLLocalDB;Database=PeopleDb;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
     </connectionStrings>
     
3. Open `PeopleApp.sln` in Visual Studio.
4. Press **F5** or click **Start Debugging** → The application will run in your browser via IIS Express.

## Using the Application

* **Add a new user:**

  1. Click **Add New Person**
  2. Fill in Full Name, Phone, Email, and Profile Image
  3. Click **Save**
  4. Invalid emails will show an error message.

* **Search people:**

  1. Enter a name in the search field
  2. Click **Search**
  3. Click **Reset** to show the full list again

* **Export PDF:**

  1. Click **Export PDF**
  2. The PDF file opens in your browser; you can save it from there.

## PDF File

* The PDF is generated based on the current list of people.
* It opens automatically in the browser when exporting.
