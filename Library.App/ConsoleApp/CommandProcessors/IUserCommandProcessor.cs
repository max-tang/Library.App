namespace Library.App.ConsoleApp
{
    /// <summary>
    /// Used to parse and execute a user input command from console.
    /// </summary>
    public interface IUserCommandProcessor
    {
        /// <summary>
        /// Process one line of user input.
        /// </summary>
        /// <param name="input">User input</param>
        Task ProcessUserInput(string input);

        /// <summary>
        /// Print out the usage for this command processor
        /// </summary>
        void PrintUsage();
    }
}
