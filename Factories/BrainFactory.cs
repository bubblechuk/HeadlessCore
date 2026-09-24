using HeadlessCore.Brains;
using HeadlessCore.Characters;

namespace HeadlessCore.Factories;

public static class BrainFactory
{
    public static BaseBrain Create(string brainTypeId, CBaseEntity owner)
    {
        return brainTypeId.ToLower() switch
        {
            "player" => new PlayerBrain(owner),
            _ => throw new ArgumentException($"Unknown BrainTypeId: {brainTypeId}")
        };
    }
}