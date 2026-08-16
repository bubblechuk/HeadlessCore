using HeadlessCore.Characters;
using HeadlessCore.Configurations;
using HeadlessCore.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                throw new DirectoryNotFoundException();
            }
            var entitiesFiles = Directory.GetFiles(path, "*.effect.json", SearchOption.AllDirectories);
            foreach (var entityFile in entitiesFiles)
            {
                try
                {
                    var entityContent = File.ReadAllText(entityFile);
                    var config = JsonSerializer.Deserialize<EffectConfig>(entityContent,
                                                                          new JsonSerializerOptions()
                                                                          {
                                                                              PropertyNameCaseInsensitive = true
                                                                          }
                    );
                    if (config != null && !string.IsNullOrEmpty(config.Id))
                    {
                        _configs[config.Id] = config;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Core] Error loading JSON file: {path}: {ex.Message}");
                }
            }
        }
        public static CStatusEffect Create(string id)
        {
            if (!_configs.TryGetValue(id, out var config) || config == null)
            {
                throw new KeyNotFoundException();
            }

            return config.ToDomain();
        }

    }
}
