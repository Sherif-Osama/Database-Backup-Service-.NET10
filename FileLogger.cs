using Microsoft.Extensions.Options;
namespace DatabaseBackupService;

public class FileLogger
{
    private readonly BackupSettings _settings;
    public FileLogger(IOptions<BackupSettings> options)
    {
        _settings = options.Value;
    }

    public void Log(string message)
    {
        try
        {
            string logFilePath = Path.Combine(_settings.LogFolder, "Log.txt");

            File.AppendAllText(
                logFilePath,
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} : {message}{Environment.NewLine}");
        }
        catch { }//
    }
}