![Build Status](https://arcooper.visualstudio.com/APITest/_apis/build/status/arcoopertestapi%20-%20CI)

# .NET Core API
A repository to utilize learning how to use the .NET Core MVC API using Entity Framework and SQL Server

This project created a new database, as well as updates the tables using Entity Framework Core

## Database Schema can be updated using the following commands:
Add-Migration
  - This will prompt for a name for the update that will be added to the `_EFMigrationHistory` table in the created database.
  - With the Refactor of the Domain into its own project, the command for adding a new migration to the project is now
  `Add-Migration "MigrationName" -Project Project-Name-Containing-Migrations`
    - An example in this project would look like `Add-Migration "Refactor" -Project APITest.Domain`

Remove-Migration
  - Removing migrations was updated in a similar manner with the architecture change, it was updated to `Remove-Migration -Project Project-Name-Containing-Migrations`
    - An example in this project would look like `Remove-Migration -Project APITest.Domain`

Update-Database
  - This command will update the schema of the database.
  - Updating the database has changed with the architecture change as well. It was updated to `Update-Database -Project Project-Name-Containing-Migrations`
    - An example in this project would look like `Update-Database -Project APITest.Domain`
  - Passing the `-Migration` argument along with a migration name will allow you to revert back to that migration. 
    - ex. `Update-Database -Migration:"InitialMigration"`

## Desired features from this test API
- Ability to add user entities
- Ability for a user to login
- CRUD operations for entities
- Persistent migrations for database
- Establishing relationships and seeding data using the Entity Framework Core Fluent API
- Authorizing users using JSON Web Tokens
- Navigation properties for related entities

## Learning Experiences
So I've been working on this small application on and off for a while, so I'll use this area to gather my thoughts on 
some of the things I've learned so far. 

1. The first thing that I learned is that the EF Core data annotation for declaring your model are powerful. 
However, they can be finicky at times too. It's sometimes beneficial to combine both the data annotations and the fluent API
to achieve the desired model.
2. JSON Web Tokens (JWT) are hard. Having no prior experience with them, I wanted to set the functionality up in this, since I eventually
want to create an Angular application to streamline the CRUD operations for the API endpoints.
3. Entity Framework is cool, but there's a lot of overhead versus a SQL statement written by anyone who has experience with it. That 
being said, EF does simplify the process of getting a database off the ground somewhat. It certainly streamlines the database CRUD
operations.
4. Since the data seeding api keeps track of the models generated when seeding, anything with a GUID kind of gets the short end of 
the stick. Seems like it would be better to generate any seed data using raw SQL statements in a migration where you would like it.
This probably applies less when using an auto-incremented integer as a primary key, but I haven't tried that.
5. Authorization claims are interesting. The authorization service pattern was an interesting way to handle authorization using 
a JWT claim. It seems as though JWT authentication libraries sometimes compensate for _"clock skew"_ which was giving me strange behavior when testing
my tokens. According to [the answer on this post](https://stackoverflow.com/questions/39728519/jwtsecuritytoken-doesnt-expire-when-it-should) the default
for the `System.IdentityModel.Tokens.Jwt` library that I am using is 5 miutes, so tokens that expire in 1 minute would expire in 6, etc. This
is consistent with the behavior that I was seeing.
6. Inversion of Control: I spoke to an architect at work about ways to improve this API. He sent me a link to his API that he had developed
for a similar reason. He referred me to look at the way he set up the services and controllers, and utilized each in a coherent way. 
When I first started I had encountered a problem that the service pattern solved. In my user controller, I wanted the user entity to also
be in control of the person's information, and wanted the add and update to take place inside of the person controller. So the services
idea was a lifesaver in that regard.
7. Async/Await pattern: For the sake of scalability I wanted to familiarize myself with it. If I did it correctly, I have no idea, but I suppose
I will find out some day. I like to think that it was done _decently_
8. *AutoMapper* has very poor documentation. However, it is a powerful tool for mapping your view model to your data model. It allowed me to be lazy and use minimal
effort to achieve what I wanted. It even applies the same profile logic to `IEnumerable` objects, which is very, _very_ nice. The setup was relatively simple, once
I found some decent guides on how to set it up. The documentation was really almost no help, though.
9. Response Serialization can sometimes get messy when your navigation properties relate back to each other and create a circular dependency. This can be ignored by
using the `opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore` line in the JSON configuration options in the `Startup.cs` file. 
10. Moving the entire context, along with its related properties, into it's own project was as simple as manually migrating those files, then adding this line to the
`ConfigureServices` method in the `Startup.cs` file:
  - `opt.UseSqlServer(connectionString, opts => opts.MigrationsAssembly("APITest.Domain"));`

## Dev Ops
This section will detail things that I have learned with the Azure Pipeline Dev Ops deployments to Azure.

1. The Azure build script for .NET Core applications are super simple boilerplate scipts. The only necessary modification I think that I made was
the dotnet restore step.
2. The release was a little more complicated. I wanted to be able to push my code up with whatever values I needed to develop locally,
as well as the secrets that were necessary to run on Azure. To do this, I used the variable substitution option on Azure Dev Ops. This allowed me to
declare variables values within my release pipeline that would be substituted into the `appsettings.json` file during the `Deploy Azure App Service` step in the Release pipeline. 
This allows me to maintain all of my variables for local development, as well as have the proper variables for Production (or really any other environment). 
3. Generating API Management documentation: I was able to generate API management documentation after deployment using an ARM template given to me by a senior architect.
This allowed me to have documentation generated for every build of my application within API management using the OpenAPI documentation generated by the Swashbuckle tool.
This updates automatically and is versioned using the build version. 