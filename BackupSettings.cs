namespace DatabaseBackupService
{
    public class BackupSettings
    {
        public string ConnectionString { get; set; }

        public string BackupFolder { get; set; }

        public string LogFolder { get; set; }

        public int BackupIntervalMinutes { get; set; }
    }
}