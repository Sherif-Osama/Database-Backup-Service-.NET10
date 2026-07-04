using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
namespace DatabaseBackupService
{
    public class Worker(IOptions<BackupSettings> options, ILogger<Worker> logger, FileLogger fileLogger) : BackgroundService
    {
        private readonly BackupSettings _settings = options.Value;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Database Backup Service Started.");
            fileLogger.Log("Database Backup Service Started.");

            Directory.CreateDirectory(_settings.BackupFolder);
            Directory.CreateDirectory(_settings.LogFolder);

            while (!stoppingToken.IsCancellationRequested)
            {
                await BackupDatabaseAsync();

                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(_settings.BackupIntervalMinutes), stoppingToken);
                }
                catch (OperationCanceledException) { break; }
            }
        }

        private async Task BackupDatabaseAsync()
        {
            try
            {
                logger.LogInformation("Backup Started.");
                fileLogger.Log("Backup Started.");

                string filePath = Path.Combine(_settings.BackupFolder, $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                logger.LogInformation("Backup File: {FilePath}", filePath);
                fileLogger.Log($"Backup File: {filePath}");

                string databaseName = new SqlConnectionStringBuilder(_settings.ConnectionString).InitialCatalog;

                logger.LogInformation("Database: {DatabaseName}", databaseName);
                fileLogger.Log($"Database: {databaseName}");

                string backupQuery = $@"BACKUP DATABASE [{databaseName}] TO DISK = N'{filePath}';";

                using SqlConnection connection = new SqlConnection(_settings.ConnectionString);
                await connection.OpenAsync();

                using SqlCommand command = new SqlCommand(backupQuery, connection);
                await command.ExecuteNonQueryAsync();

                logger.LogInformation("Database backup completed successfully.");
                fileLogger.Log("Database backup completed successfully.");
            }
            catch (Exception ex) { logger.LogError(ex, "Database backup failed."); fileLogger.Log($"Database backup failed: {ex}"); }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Database Backup Service Stopped.");
            fileLogger.Log("Database Backup Service Stopped.");

            await base.StopAsync(cancellationToken);
        }
    }
}