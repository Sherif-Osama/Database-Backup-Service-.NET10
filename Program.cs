namespace DatabaseBackupService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.Configure<BackupSettings>(builder.Configuration.GetSection("BackupSettings"));
            builder.Services.AddSingleton<FileLogger>();

            if (OperatingSystem.IsWindows())
            {
                builder.Services.AddWindowsService(options =>
                {
                    options.ServiceName = "Database Backup Service";
                });
            }

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}