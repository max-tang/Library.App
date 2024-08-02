# Prerequisite for building and running the application locally

1. .NET 6 installed

# Build the Application and tests

```sh
dotnet build
```

# Run the tests

```sh
dotnet test
```

# Run the Console application

```sh
dotnet run --project Library.App
```

# Example commands within the Console application

General format of supported commands is `COMMAND_KEY [BODY]`. COMMAND_KEY is case insensitive. Format of body is command specific. `LIST` does not need a body, `ADD` and `UPDATE` need a JSON body, while `SHOW` and `REMOVE` need a 64 bit integer id as body.

```sh
# Add a new book
ADD  {"Isbn":"0-061-96436-0","Title":"Big event","Author":"John Doe","PublishDate":"2024-01-20","Description":"A brief of the book"}

ADD {"Isbn":"0-061-96436-1","Title":"Big event","Author":"John Doe"}

# print out existing books
LIST

# Update an existing book
UPDATE  {"Id": 1, "Isbn":"0-061-96436-0","Title":"Big event","Author":"John Bell","PublishDate":"2024-04-21","Description":"Some new description"}

# Show details of a specific book
SHOW 1

# Remove a book
REMOVE 1

```

# Project structure

The Library.App has 2 main top level folders holding 2 subsystems. The `Domain` folder contains business logic, while `ConsoleApp` folder contains the presentation layer of the Application.

Instead of a layered structure, the `Domain` folder is designed to use a vertical slice pattern. Code components related to a business feature are grouped together, so that changes to one feature will have minimal impact on ohter features.

```
-------Library.App
        |
        |___ Domain
        |    |
        |    |___Books
        |    |
        |    |___(Orders) (May add in the future)
        |
        |___ConsoleApp

```

## Service registration and creation

Services are regiestered and created using .NET core's dependency injection container. More specifically, in this small project, we need to inject `IBookService` for our `IUserCommandProcessor` instances. DI container to manage all of them. As the underlying `DBContext` is designed to be short lived, services depending on it should not out-live the scope of `DBContext`. We create a `IServiceScope` instance for each user command, and create our services in the scope.

## Using contract classes (Data Transfer Object)

Under folder `Domain/Books/Commands` and `Domain/Books/Queries`, a few classes forms the contracts we provide to the other layers of the application. This has the benefit of allowing internal implementation involution without breaking users of the business logic layer.

## More details about the presentation layer

To support the extension of user commands, a Dynamic Factory pattern is applied. The `Application` class has a `Dictionary _registeredProcessors` field, holding all currently regiestered CommandProcessors. At run time, the `Application` instance uses this information to create required Command processors at run time, based on user input.
This pattern applies well here. If we need to add a new Command processor in the future, all we need to do is to implement the `IUserCommandProcessor` interface, or create a subclass of `BaseUserCommandProcessor`, and register it to the Dependency Injection Container and the `Application` instance. This aligns well with the Open/Closed Principle.

I also considered the Chain of Responsiblity pattern. Basically, let the `Application` class hold a list of `IUserCommandProcessor`, and when a new user input comes, the `Application` instance asks each of these `IUserCommandProcessor` implementors if they can handle the command, until one that can handle it is found. There are two drawbacks for this approach though.

1. It is not very efficient when the number of supported commands grows very large.
2. More importantly, we have to create all these `IUserCommandProcessor` ahead of time. Given that these command processors all depend on our `IBookService` and in turn depends on `IBookRepository`, which we registered as `Scoped` in the DI container. If we hold these instances in `Application` instance, our `IBookService` and `IBookRepository` instances will out-live their designed lifetime, that causes higher chance of getting Database concurrency issues.
