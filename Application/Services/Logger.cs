using Domain.Interfaces.Services;

namespace Application.Services
{
    public class Logger : ILogger
    {
        private readonly string _path;
        public Logger(string path)
        {
            _path = path;
        }

        public void Log(string message)
        {
            try
            {
                File.AppendAllText(_path, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing log: {ex.Message}");
            }
        }
    }
}
