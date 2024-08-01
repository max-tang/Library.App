
using System.Text.Json;
using Library.App.Books;

namespace Library.App.ConsoleApp
{
    /// <summary>
    /// Base class for all command processors, provides facilities like parsing command body from JSON.
    /// </summary>
    /// <typeparam name="T">Type of expected command body.</typeparam>
    abstract class BaseUserCommandProcessor<T> : IUserCommandProcessor where T : class
    {
        protected IBookService _bookService;

        protected string CommandKey { get; set; }

        protected virtual T ParseCommandBody(string body)
        {
            try
            {
                T command = JsonSerializer.Deserialize<T>(body);
                return command;
            }
            catch (JsonException)
            {
                Console.WriteLine($"Invalid json, please check your input.");
            }
            return default;
        }

        public BaseUserCommandProcessor(IBookService bookService, string commandKey)
        {
            _bookService = bookService;
            CommandKey = commandKey;
        }

        public async Task ProcessUserInput(string input)
        {
            if (input.Length < CommandKey.Length)
            {
                return;
            }

            if (input.Substring(0, CommandKey.Length).ToLower() == CommandKey)
            {
                T command = ParseCommandBody(input.Length > CommandKey.Length ? input[CommandKey.Length..] : "");
                if (command == null)
                {
                    Console.WriteLine("Failed to parse command, usage:");
                    PrintUsage();
                    return;
                }

                await DoProcess(command);
            }
        }

        protected abstract Task DoProcess(T command);

        public abstract void PrintUsage();
    }

}