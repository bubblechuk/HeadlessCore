namespace HeadlessCore.Parties;
using HeadlessCore.Characters;
public class PlayerParty
{
    private readonly List<CBaseEntity>  _entities;
    public IReadOnlyList<CBaseEntity> Members => _entities;

    public PlayerParty(IEnumerable<CBaseEntity> entities)
    {
        _entities = new List<CBaseEntity>(entities);
    }

    public PlayerParty()
    {
    }

    public void AddMember(CBaseEntity member)
    {
        if (_entities.Any(c => c.Id == member.Id))
        {
            CoreLogger.LogError($"Entity {member.Id} is already in party");
            return;
        }

        _entities.Add(member);
    }
}