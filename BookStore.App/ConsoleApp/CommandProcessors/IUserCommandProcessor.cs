namespace BookStore.App.ConsoleApp
{
    interface IUserCommandProcessor
    {
        Task<bool> ProcessUserInput(string input);
        void PrintUsage();
    }
}
