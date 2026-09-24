using System.Text.Json;
using System.Text.Json.Serialization;
using HeadlessCore.Characters;
using HeadlessCore.Factories;
using HeadlessCore.Parties;

namespace HeadlessCore.Saves;

public static class SaveManager
{
    public const int MaxSlots = 4;
    public static SaveData?[] Slots { get; private set; } = new SaveData?[MaxSlots];
    public static int CurrentSlotIndex { get; private set; } = -1;
    public static PlayerParty? CurrentPlayerParty { get; private set; }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
    public static void FetchSaves(string path = "save")
    {
        Array.Clear(Slots, 0, Slots.Length);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            CoreLogger.LogDebug($"Save directory missing. Created directory: \"{path}\"");
            return;
        }
        for (int i = 0; i < MaxSlots; i++)
        {
            string filePath = Path.Combine(path, $"slot_{i}.save.json");
            if (File.Exists(filePath))
            {
                try
                {
                    string content = File.ReadAllText(filePath);
                    var saveData = JsonSerializer.Deserialize<SaveData>(content, JsonOptions);
                    if (saveData != null)
                    {
                        Slots[i] = saveData;
                        CoreLogger.LogDebug($"Slot {i} loaded: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    CoreLogger.LogError($"Error loading save slot {i}: {ex.Message}");
                }
            }
        }
    }
    public static void NewGame(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= MaxSlots) return;

        CurrentSlotIndex = slotIndex;
        var newPlayer = EntityFactory.Create("player");
        CurrentPlayerParty = new PlayerParty(new List<CBaseEntity> { newPlayer });

        CoreLogger.LogDebug($"New game started in slot {slotIndex}");
    }
    public static bool LoadSave(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= MaxSlots || Slots[slotIndex] == null)
        {
            CoreLogger.LogError($"Cannot load empty or invalid slot: {slotIndex}");
            return false;
        }

        CurrentSlotIndex = slotIndex;
        var selectedSave = Slots[slotIndex]!;
        CurrentPlayerParty = selectedSave.Party;
        
        CoreLogger.LogDebug($"Loaded save from slot {slotIndex} (ID: {selectedSave.Id})");
        return true;
    }
    public static void SaveCurrentGame(string path = "save")
    {
        if (CurrentSlotIndex < 0 || CurrentSlotIndex >= MaxSlots)
        {
            CoreLogger.LogError("Cannot save: No active slot selected.");
            return;
        }

        SaveSlot(CurrentSlotIndex, path);
    }
    public static void SaveSlot(int slotIndex, string path = "save")
    {
        if (CurrentPlayerParty == null)
        {
            CoreLogger.LogError("Cannot save: No active player party found.");
            return;
        }

        var saveData = new SaveData
        {
            Id = $"slot_{slotIndex}",
            Party = CurrentPlayerParty
        };
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, $"slot_{slotIndex}.save.json");
            string jsonString = JsonSerializer.Serialize(saveData, JsonOptions);
            
            File.WriteAllText(filePath, jsonString);
            Slots[slotIndex] = saveData;
            
            CoreLogger.LogDebug($"Game saved successfully to slot {slotIndex}");
        }
        catch (Exception ex)
        {
            CoreLogger.LogError($"Failed to write save file for slot {slotIndex}: {ex.Message}");
        }
    }
}