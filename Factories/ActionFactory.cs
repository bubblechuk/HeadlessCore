using HeadlessCore.Actions;
using HeadlessCore.Configurations;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                throw new DirectoryNotFoundException($"Directory \"{path}\" not found.");
            }
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            var actionsFiles = Directory.GetFiles(path, "*.action.json", SearchOption.AllDirectories);
            foreach (var actionFile in actionsFiles)
            {
                try
                {
                    var actionContent = File.ReadAllText(actionFile);
                    var config = JsonSerializer.Deserialize<ActionConfig>(actionContent,
                                                                          jsonOptions
                    );
                    if (config != null && !string.IsNullOrEmpty(config.Id))
                    {
                        _configs[config.Id] = config;
                    }
                    CoreLogger.LogDebug($"{Path.GetRelativePath(path, actionFile)} loaded");
                }
                catch (Exception ex)
                {
                    CoreLogger.LogError($"Error loading JSON file: {actionFile}: {ex.Message}");
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
