using HeadlessCore.Characters;
using HeadlessCore.Configurations;
using System.Text.Json;

namespace HeadlessCore.Factories
{
    public static class EntityFactory
    {
        public static Dictionary<string, EntityConfig> _configs = new();
        public static bool IsInitialized { get; private set; }
        public static void Initialize(string path)
        {
            _configs.Clear();
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }
            var entitiesFiles = Directory.GetFiles(path, "*.entity.json", SearchOption.AllDirectories);
            foreach (var entityFile in entitiesFiles)
            {
                try
                {
                    var entityContent = File.ReadAllText(entityFile);
                    var config = JsonSerializer.Deserialize<EntityConfig>(entityContent,
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
                catch (Exception ex) {
                    Console.WriteLine($"[EntityFactory] Error loading JSON file: {path}: {ex.Message}");
                }
            }
        }
        public static CBaseEntity Create(string id)
        {
            if (!_configs.TryGetValue(id, out var config) || config == null)
            {
                throw new KeyNotFoundException();
            }
            var entity = new CBaseEntity(config);
            return entity;
        }
    }
}
