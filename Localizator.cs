using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HeadlessCore
{
    public static class Localizator
    {
        private static readonly Dictionary<string, string> _localeFiles = new();

        private static readonly Dictionary<string, string> _translations = new();
        public static string CurrentLocale { get; private set; } = string.Empty;
        public static IReadOnlyCollection<string> Locales => _localeFiles.Keys;

        public static void Initialize(string path)
        {
            _localeFiles.Clear();

            if (!Directory.Exists(path))
            {
                CoreLogger.LogError($"[Localizator] Directory not found: {path}");
                return;
            }

            var files = Directory.GetFiles(path, "*.locale.json");

            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                string langCode = fileName.Split('.')[0];

                _localeFiles[langCode] = file;
            }

            if (_localeFiles.Count == 0)
            {
                CoreLogger.LogWarning($"[Localizator] No *.locale.json files found in {path}");
                return;
            }
            CoreLogger.LogInfo($"[Localizator] Found locales: {string.Join(", ", Locales)}");
        }

        public static bool SetLocale(string lang)
        {
            if (!_localeFiles.TryGetValue(lang, out var filePath))
            {
                CoreLogger.LogError($"[Localizator] Locale '{lang}' is not available!");
                return false;
            }

            try
            {
                string jsonText = File.ReadAllText(filePath);

                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText);

                _translations.Clear();
                if (data != null)
                {
                    foreach (var (key, value) in data)
                    {
                        _translations[key] = value;
                    }
                }

                CurrentLocale = lang;
                CoreLogger.LogInfo($"[Localizator] Locale changed to '{lang}'");
                return true;
            }
            catch (Exception ex)
            {
                CoreLogger.LogError($"[Localizator] Failed to parse locale file '{filePath}': {ex.Message}");
                return false;
            }
        }
        public static string Get(string key)
        {
            if (_translations.TryGetValue(key, out var translation))
            {
                return translation;
            }
            CoreLogger.LogWarning($"[Localizator] Missing key '{key}' in locale '{CurrentLocale}'");
            return key;
        }
        public static string Get(string key, params object[] args)
        {
            string rawText = Get(key);
            try
            {
                return string.Format(rawText, args);
            }
            catch
            {
                return rawText;
            }
        }
    }
}