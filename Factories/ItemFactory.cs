using HeadlessCore.Configurations;
using HeadlessCore.Items;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeadlessCore.Factories
{
    public static class ItemFactory
    {
        private static readonly Dictionary<string, ItemConfig> _configs = new();
        public static bool IsInitialized { get; private set; }

        public static void Initialize(string path)
        {
            _configs.Clear();
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory \"{path}\" not found.");
            }

            var itemFiles = Directory.GetFiles(path, "*.item.json", SearchOption.AllDirectories);
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            foreach (var file in itemFiles)
            {
                try
                {
                    var content = File.ReadAllText(file);
                    var config = JsonSerializer.Deserialize<ItemConfig>(content, jsonOptions);

                    if (config != null && !string.IsNullOrEmpty(config.Id))
                    {
                        _configs[config.Id] = config;
                    }
                    CoreLogger.LogDebug($"{Path.GetRelativePath(path, file)} loaded");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file {file}: {ex.Message}");
                }
            }

            IsInitialized = true;
        }

        public static CBaseItem Create(string id, int count = 1)
        {
            if (!_configs.TryGetValue(id, out var config) || config == null)
            {
                throw new KeyNotFoundException($"Item config with ID '{id}' was not found.");
            }

            return config.Type switch
            {
                "Consumable" => new ConsumableItem(
                    config.Id,
                    config.ActionId ?? throw new InvalidOperationException($"Consumable item '{id}' missing ActionId"),
                    config.MaxStackSize,
                    currentStack: count
                ),

                "Equipment" => new EquipmentItem(
                    config.Id,
                    config.Slot,
                    config.BonusStats.ToDomainStats()
                ),

                _ => new CDefaultItem(
                    config.Id,
                    config.MaxStackSize,
                    currentStack: count
                )
            };
        }
    }
}