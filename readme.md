# Prerequests for building and running the application locally

1. .NET Core 6.0 installed

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

Instead of a layered structure, the `Domain` folder is designed to use a vertical slice pattern. Code components related to a business feature are grouped together, so that changes to one feature will have minimal impact to ohter features.

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

Services are regiestered and created using .NET core's dependency injection container.

## More details about the presentation layer

To support the extension of user commands, a Dynamic Factory pattern is applied. The `Application` class has a `Dictionary _registeredProcessors` field, holding all currently regiestered CommandProcessors. At run time, the `Application` instance uses this information to create required Command processors at run time, based on user input.
This pattern applies well here. If we need to add a new Command processor in the future, all we need to do is to implement the `IUserCommandProcessor` interface, or create a subclass of `BaseUserCommandProcessor`, and register it to the Dependency Injection Container and the `Application` instance. This aligns well with the Open/Closed Principle.

I also tried the Chain of Responsiblity pattern at first. Basically, let the `Application` class hold a list of `IUserCommandProcessor`, and when a new user input comes, the `Application` instance asks each of these `IUserCommandProcessor` implementors if they can handle the command, until one that can handle it is found. There are two drawbacks for this approach though. Firstly, it is not very efficient when the number of supported commands grows very large. Secondly, and more importantly, we have to create all these `IUserCommandProcessor` ahead of time. Given that these command processors all depends on our `IBookService`, which we registered as `Scoped` in the DI container. It will break our designed lifetime of `IBookService`, which causes higher chance of Database concurrency issues.

# TODO

1. ISBN validation
2. documentation
3. Unit Testing
