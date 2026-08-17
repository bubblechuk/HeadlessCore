using HeadlessCore.Configurations;
using HeadlessCore.Actions;
using System.Text.Json;

namespace HeadlessCore.Factories
{
    public static class ActionFactory
    {
        public static Dictionary<string, ActionConfig> _configs = new();
        public static bool IsInitialized { get; private set; }
        public static void Initialize(string path)
        {
            _configs.Clear();
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }
            var entitiesFiles = Directory.GetFiles(path, "*.action.json", SearchOption.AllDirectories);
            foreach (var entityFile in entitiesFiles)
            {
                try
                {
                    var entityContent = File.ReadAllText(entityFile);
                    var config = JsonSerializer.Deserialize<ActionConfig>(entityContent,
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
                    Console.WriteLine($"[ActionFactory] Error loading JSON file: {path}: {ex.Message}");
                }
            }
        }
        public static IAction Create(string id)
        {
            if (!_configs.TryGetValue(id, out var config) || config == null)
            {
                throw new KeyNotFoundException();
            }
            var effects = config.EffectIds
                .Select(effectId => EffectFactory.Create(effectId))
                .ToList();
            return config.Scope switch
            {
                //"RandomTargets" => new CRandomTargetAction(
                //    config.Id,
                //    config.Name,
                //    config.Description,
                //    config.Cost,
                //    config.ActionType,
                //    config.BaseValue,
                //    config.StatScaling,
                //    hitsCount: 3,
                //    effects
                //),

                _ => new DefaultAction(
                    config.Id,
                    config.Name,
                    config.Description,
                    config.Cost,
                    config.Scope,
                    config.ActionType,
                    config.BaseValue,
                    config.StatScaling,
                    effects
                )
            };
        }

    }
}
