using HeadlessCore.Configurations;
using HeadlessCore.Effects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeadlessCore.Factories
{
    public static class EffectFactory
    {
        public static Dictionary<string, EffectConfig> _configs = new();
        public static bool IsInitialized { get; private set; }
        public static void Initialize(string path)
        {
            _configs.Clear();
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory \"{path}\" not found");
            }
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            var effectsFiles = Directory.GetFiles(path, "*.effect.json", SearchOption.AllDirectories);
            foreach (var effectFile in effectsFiles)
            {
                try
                {
                    var effectContent = File.ReadAllText(effectFile);
                    var config = JsonSerializer.Deserialize<EffectConfig>(effectContent,
                                                                          jsonOptions
                    );
                    if (config != null && !string.IsNullOrEmpty(config.Id))
                    {
                        _configs[config.Id] = config;
                    }
                    CoreLogger.LogDebug($"{Path.GetRelativePath(path, effectFile)} loaded");
                }
                catch (Exception ex)
                {
                    throw new JsonException($"Error loading JSON file: {effectFile}: {ex.Message}");
                }
            }
        }
        public static CStatusEffect Create(string id)
        {
            if (!_configs.TryGetValue(id, out var config) || config == null)
            {
                throw new KeyNotFoundException($"Effect \"{id}\" not found");
            }

            return config.ToDomain();
        }

    }
}
