namespace HeadlessCore.Items
{
    public interface IConsumable
    {
        bool Use(ITargetable caster, ITargetable target);
    }
}
