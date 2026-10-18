
namespace CustomerBeneficiaryManagement.Logging
{
    public class ApiLogger
    {
        private readonly string _logFolder;

        public ApiLogger(IConfiguration configuration)
        {
            _logFolder = configuration["LogSettings:FolderPath"] ?? "Logs";
        }

        public async Task LogAsync(string message, string level = "INFO")
        {
            string dateFolder = Path.Combine(
                _logFolder,
                DateTime.Now.ToString("yyyy-MM-dd"));

            Directory.CreateDirectory(dateFolder);

            string filePath = Path.Combine(dateFolder, "api.log");

            string logMessage =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";

            await File.AppendAllTextAsync(filePath, logMessage);
        }
    }
}
