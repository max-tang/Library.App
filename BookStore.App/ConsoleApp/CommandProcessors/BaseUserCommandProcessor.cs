
using System.Text.Json;
using BookStore.App.Books;

namespace BookStore.App.ConsoleApp
{
    abstract class BaseUserCommandProcessor<T> : IUserCommandProcessor
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

        public async Task<bool> ProcessUserInput(string input)
        {
            if (input.Length < CommandKey.Length)
            {
                return false;
            }

            if (input.Substring(0, CommandKey.Length).ToLower() == CommandKey)
            {
                T command = ParseCommandBody(input.Length > CommandKey.Length ? input.Substring(CommandKey.Length) : "");
                if (command == null)
                {
                    Console.WriteLine("Failed to parse command, usage:");
                    PrintUsage();
                    return true;
                }

                try
                {
                    await DoProcess(command);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine($"Failed to process command: {input}");
                }

                return true;
            }
            return false;
        }

        protected abstract Task DoProcess(T command);

        public abstract void PrintUsage();
    }

}