# Database Backup Service (.NET 8 Worker Service)

A Windows Worker Service built with **.NET 10** that automatically creates scheduled backups for SQL Server databases. The service runs continuously in the background, performs backups at configurable intervals, and logs all operations using both the built-in logging system and a custom file logger.

---

## Features

- Automatic SQL Server database backups
- Configurable backup interval
- Background execution using Worker Service
- Configuration with `appsettings.json`
- Dependency Injection (DI)
- Options Pattern (`IOptions<T>`)
- Built-in logging with `ILogger`
- Custom file logging
- Graceful shutdown using `CancellationToken`
- Windows Service support
- Fully asynchronous database operations (`async/await`)

---

## Technologies

- C#
- .NET 10
- Worker Service
- SQL Server
- Microsoft.Data.SqlClient
- Dependency Injection
- Options Pattern
- Microsoft.Extensions.Logging

---

## Project Structure

```text
DatabaseBackupService
│
├── BackupSettings.cs
├── FileLogger.cs
├── Worker.cs
├── Program.cs
├── appsettings.json
├── InstallService.bat
├── UninstallService.bat
└── DatabaseBackupService.csproj
```

---

## Configuration

The application is configured using `appsettings.json`.

```json
{
  "BackupSettings": {
    "ConnectionString": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True",
    "BackupFolder": "C:\\DatabaseBackups",
    "LogFolder": "C:\\DatabaseBackups\\Logs",
    "BackupIntervalMinutes": 60
  }
}
```

| Setting | Description |
|---------|-------------|
| ConnectionString | SQL Server connection string |
| BackupFolder | Directory where backup files are stored |
| LogFolder | Directory where log files are stored |
| BackupIntervalMinutes | Time between backups (minutes) |

---

## How It Works

1. The Worker Service starts.
2. Configuration is loaded from `appsettings.json`.
3. Backup and log directories are created automatically.
4. The service executes a SQL Server `BACKUP DATABASE` command.
5. Every operation is logged using both `ILogger` and a custom file logger.
6. The service waits for the configured interval.
7. The process repeats until the service is stopped.

---

## Installing as a Windows Service

1. Build the project in **Release** mode.
2. Copy all generated files to a folder.
3. Run `InstallService.bat` as **Administrator**.

To uninstall the service, run:

```text
UninstallService.bat
```

---

## Logging

The application records all important operations, including:

- Service startup
- Backup start
- Database name
- Backup file path
- Successful backups
- Errors and exceptions
- Service shutdown

Example:

```text
2026-07-04 10:30:15 : Database Backup Service Started.
2026-07-04 10:30:15 : Backup Started.
2026-07-04 10:30:15 : Database: DVLD
2026-07-04 10:30:17 : Database backup completed successfully.
```

---

## Error Handling

- SQL Server exceptions are caught and logged.
- Backup failures do not stop the service.
- Graceful shutdown is handled using `CancellationToken`.

---

## Future Improvements

- Backup compression
- Backup retention policy
- Automatic cleanup of old backups
- Email notifications
- Restore functionality
- Support for multiple databases

---

## Author

**Sherif Osama**
