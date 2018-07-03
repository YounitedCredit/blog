namespace DependencyInjectionTest.Services
{
    public interface ILogger
    {

    }

    public class Logger : ILogger
    {

    }
    
    public interface IRepository
    {
        bool IsSimulation { get; }
    }

    public class Repository : IRepository
    {
        private ILogger _logger;
        public bool IsSimulation { get; private set; }

        public Repository(ILogger logger, bool isSimulation)
        {
            _logger = logger;
            IsSimulation = isSimulation;
        }
    }

}
