using HeadlessCore.Characters;
using HeadlessCore.Parties;

namespace HeadlessCore.Combat
{
    public enum BattleState
    {
        NotStarted,
        InProgress,
        PlayerVictory,
        EnemyVictory,
        Fled
    }
    public class BattleSession
    {
        public IParty Allies { get; }
        public IParty Enemies { get; }
        public event Action<BattleState> OnBattleStateChanged;
        public event Action<CBaseEntity> OnTurnStarted;
        public event Action<CBaseEntity> OnTurnEnded;

        public CBaseEntity? CurrentEntity { get; private set; }
        public int TurnNumber { get; private set; } = 0;
        public BattleState State { get; private set; } = BattleState.NotStarted;
        private readonly Queue<CBaseEntity> _turnQueue = new();
        public BattleSession(IParty allies, IParty enemies)
        {
            Allies = allies ?? throw new ArgumentNullException(nameof(allies));
            Enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
            if (!Allies.Members.Any() || !Enemies.Members.Any())
            {
                throw new ArgumentException("Both teams must have at least one member");
            }

            foreach (var member in Allies.Members.Concat(Enemies.Members))
            {
                member.OnDeath += CheckBattleStatus;
            }
        }
        public void CheckBattleStatus()
        {
            if (State != BattleState.InProgress) return;

            bool enemiesAlive = Enemies.Members.Any(m => !m.IsDead);
            bool alliesAlive = Allies.Members.Any(m => !m.IsDead);

            if (!enemiesAlive) EndBattle(BattleState.PlayerVictory);
            else if (!alliesAlive) EndBattle(BattleState.EnemyVictory);
        }
        public void StartBattle()
        {
            if (State != BattleState.NotStarted) return;

            State = BattleState.InProgress;
            OnBattleStateChanged?.Invoke(State);

            RebuildTurnQueue();
            ProcessNextTurnAsync();
        }
        public void ExecuteAction(string actionId, IReadOnlyList<ITargetable> targets)
        {
            if (State != BattleState.InProgress) return;
            if (CurrentEntity == null) return;

            CurrentEntity.UseAction(actionId, targets);
        }
        public async Task ProcessNextTurnAsync(CancellationToken cancellationToken = default)
        {
            if (State != BattleState.InProgress) return;

            if (_turnQueue.Count == 0)
            {
                TurnNumber++;
                RebuildTurnQueue();
            }
            while (_turnQueue.Count > 0)
            {
                var candidate = _turnQueue.Dequeue();
                if (!candidate.IsDead)
                {
                    CurrentEntity = candidate;
                    var (actionId, targets) = await candidate.Brain.DecideTurnAsync(this, cancellationToken);
                    ExecuteAction(actionId, targets);
                    OnTurnStarted?.Invoke(CurrentEntity);
                    return;
                }
            }

            CheckBattleStatus();
            if (State == BattleState.InProgress)
            {
                ProcessNextTurnAsync();
            }
        }
        private void EndCurrentTurn()
        {
            if (CurrentEntity == null) return;

            var lastEntity = CurrentEntity;
            CurrentEntity = null;

            OnTurnEnded?.Invoke(lastEntity);

            CheckBattleStatus();

            if (State == BattleState.InProgress)
            {
                ProcessNextTurnAsync();
            }
        }
        private void RebuildTurnQueue()
        {
            _turnQueue.Clear();
            var sortedMembers = Allies.Members
                .Concat(Enemies.Members)
                .Where(m => !m.IsDead)
                .OrderByDescending(m => m.TotalStats.Willpower)
                .ToList();

            foreach (var entity in sortedMembers)
            {
                _turnQueue.Enqueue(entity);
            }
        }

        private void EndBattle(BattleState finalState)
        {
            State = finalState;
            _turnQueue.Clear();
            CurrentEntity = null;

            foreach (var entity in Allies.Members.Concat(Enemies.Members))
            {
                entity.OnDeath -= CheckBattleStatus;
            }

            OnBattleStateChanged?.Invoke(State);
        }
    }
}
