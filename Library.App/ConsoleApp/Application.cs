using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.App.ConsoleApp
{
    public class Application
    {
        private const string EXIT_COMMAND = "exit";

        private bool _exiting = false;

        private ServiceProvider _serviceProvider;

        private IDictionary<string, Type> _registeredProcessors = new Dictionary<string, Type>();

        private ILogger<Application> _logger;

        public Application(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<Application>>();
        }

        public void RegisterCommandProcessor<T>(string key) where T : IUserCommandProcessor
        {
            _logger.LogInformation($"Registering command {key} with {typeof(T)}");
            _registeredProcessors.Add(key, typeof(T));
        }

        public async void Run()
        {
            PrintWelcomeMessage();

            while (!_exiting)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input.Trim() == "")
                {
                    continue;
                }
                else if (input.ToLower() == EXIT_COMMAND)
                {
                    _exiting = true;
                }
                else
                {
                    string commandKey = input.Split(" ")[0].ToLower();

                    using IServiceScope scope = _serviceProvider.CreateScope();
                    {
                        IUserCommandProcessor userCommandProcesser = CreateProcessorFor(scope, commandKey);

                        if (userCommandProcesser == null)
                        {
                            PrintCommandNotSupportedMessage(commandKey);
                        }
                        else
                        {
                            try
                            {
                                await userCommandProcesser.ProcessUserInput(input);
                            }
                            catch (FluentValidation.ValidationException validationException)
                            {
                                Console.WriteLine(validationException.Message);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Oops, something went wrong!");
                                _logger.LogError(e, "Failed to process user command");
                            }
                        }
                    }
                }
            }
        }

        private IUserCommandProcessor CreateProcessorFor(IServiceScope scope, string key)
        {
            _logger.LogInformation($"Creating processor for {key}");
            if (_registeredProcessors.ContainsKey(key))
            {
                Type type = _registeredProcessors[key];
                try
                {
                    IUserCommandProcessor userCommandProcessor = scope.ServiceProvider.GetRequiredService(type) as IUserCommandProcessor;
                    _logger.LogInformation(userCommandProcessor.GetType() + "created");
                    return userCommandProcessor;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to create command processor for {key}");
                }
            }
            return null;
        }

        private void PrintCommandNotSupportedMessage(string commandKey)
        {
            Console.WriteLine($"Unkown command '{commandKey}'");
            PrintSupportedCommands();
        }

        private void PrintWelcomeMessage()
        {
            Console.WriteLine($"Welcome to the Book Store Console Application.");
            PrintSupportedCommands();
        }

        private void PrintSupportedCommands()
        {
            var supportedCommands = _registeredProcessors.Keys.ToList();
            supportedCommands.Add(EXIT_COMMAND);
            supportedCommands.Sort();
            Console.WriteLine($"Supported commands are:\n{string.Join(", ", supportedCommands)}");
        }
    }
}
