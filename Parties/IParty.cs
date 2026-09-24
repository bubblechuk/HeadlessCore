using HeadlessCore.Characters;

namespace HeadlessCore.Parties;

public interface IParty
{
    public IReadOnlyList<CBaseEntity> Members { get; }
}