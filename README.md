# dotnetCoreApi
A repository to utilize learning how to use the .NET Core MVC API using Entity Framework and SQL Server

This project created a new database, as well as updates the tables using Entity Framework Core

Database Schema can be updated using the following commands:
Add-Migration
  - This will prompt for a name for the update that will be added to the \_EFMigrationHstory table in the created database.
Update-Database
  - This command will update the schema of the database.
