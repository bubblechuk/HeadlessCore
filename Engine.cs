using HeadlessCore.Factories;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using HeadlessCore.Saves;

namespace HeadlessCore
{
    public static class Engine
    {
        public static void Initialize(
            string path,
            LogLevel logLevel = LogLevel.Info,
            bool catchExceptions = true,
            bool createFolders = false)
        {
            CoreLogger.MinimumLevel = logLevel;

            CoreLogger.LogInfo("HeadlessCore v0.1");
            CoreLogger.LogDebug($"catchExceptions: {catchExceptions}");
            CoreLogger.LogDebug($"createFolders: {createFolders}");
            CoreLogger.LogInfo("Initializing engine...");

            if (catchExceptions)
            {
                SetupGlobalErrorHandler();
            }

            string baseDirectory = createFolders
                ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppDomain.CurrentDomain.BaseDirectory
                : Directory.GetCurrentDirectory();

            string targetPath = Path.IsPathRooted(path)
                ? path
                : Path.Combine(baseDirectory, path);

            EnsureDirectoriesExist(targetPath);
            
            Localizator.Initialize(Path.Combine(targetPath, "locales"));
            EffectFactory.Initialize(Path.Combine(targetPath, "effects"));
            ActionFactory.Initialize(Path.Combine(targetPath, "actions"));
            ItemFactory.Initialize(Path.Combine(targetPath, "items"));
            EntityFactory.Initialize(Path.Combine(targetPath, "entities"));
            SaveManager.FetchSaves();
        }

        private static void EnsureDirectoriesExist(string basePath)
        {
            string[] subDirectories = { "effects", "actions", "items", "entities" };

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
                CoreLogger.LogDebug($"Created root directory: {basePath}");
            }

            foreach (var subDir in subDirectories)
            {
                string fullSubDirPath = Path.Combine(basePath, subDir);
                if (!Directory.Exists(fullSubDirPath))
                {
                    Directory.CreateDirectory(fullSubDirPath);
                    CoreLogger.LogDebug($"Created sub-directory: {fullSubDirPath}");
                }
            }
        }

        private static void SetupGlobalErrorHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var frame = stackTrace.GetFrame(0);

                    if (frame != null)
                    {
                        string? fileName = Path.GetFileName(frame.GetFileName());
                        int lineNumber = frame.GetFileLineNumber();
                        int columnNumber = frame.GetFileColumnNumber();
                        string? methodName = frame.GetMethod()?.Name;

                        string errorDetails = $"{fileName}({lineNumber},{columnNumber}): {ex.Message}\n";

                        CoreLogger.LogError(errorDetails);
                    }
                    else
                    {
                        CoreLogger.LogError($"[E] {ex.Message}");
                    }
                }

                Environment.Exit(1);
            };
        }
    }
}