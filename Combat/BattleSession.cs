using HeadlessCore.Characters;

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
        private readonly List<CBaseEntity> _allies = new();
        private readonly List<CBaseEntity> _enemies = new();
        public CBaseEntity? CurrentEntity { get; private set; }
        public int TurnNumber { get; private set; } = 0;
        public BattleState State { get; private set; } = BattleState.NotStarted;
        private readonly Queue<CBaseEntity> _turnQueue = new();
        public IReadOnlyList<CBaseEntity> Allies => _allies.AsReadOnly();
        public IReadOnlyList<CBaseEntity> Enemies => _enemies.AsReadOnly();
        public BattleSession(IEnumerable<CBaseEntity> allies, IEnumerable<CBaseEntity> enemies)
        {
            _enemies.AddRange(enemies);
            _allies.AddRange(allies);
            if (allies == null || enemies == null) 
            {
                throw new ArgumentException("Both teams must have at least one member");
            }
            foreach (var member in allies)
            {
                member.OnDeath += CheckBattleStatus;
            }
            foreach (var member in enemies)
            {
                member.OnDeath += CheckBattleStatus;
            }
        }
        public void CheckBattleStatus()
        {
            bool enemiesAlive = Enemies.Any(m => !m.IsDead);
            bool alliesAlive = Allies.Any(m => !m.IsDead);
        }
        public void StartBattle()
        {
            if (State != BattleState.NotStarted) return;

            State = BattleState.InProgress;
            //OnBattleStateChanged?.Invoke(State);

            RebuildTurnQueue();
            NextTurn();
        }
        public void NextTurn()
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


                    //OnTurnStarted?.Invoke(CurrentEntity);
                    return;
                }
            }

            CheckBattleStatus();
            if (State == BattleState.InProgress)
            {
                NextTurn();
            }
        }
        private void EndCurrentTurn()
        {
            if (CurrentEntity == null) return;

            var lastEntity = CurrentEntity;
            CurrentEntity = null;

            //OnTurnEnded?.Invoke(lastEntity);

            CheckBattleStatus();

            if (State == BattleState.InProgress)
            {
                NextTurn();
            }
        }
        private void RebuildTurnQueue() 
        {
            _turnQueue.Clear();
            var sortedMembers = _allies.Concat(_enemies).Where(m => !m.IsDead)
                                      .OrderByDescending(m => m.TotalStats.Willpower).ToList();
            foreach (var entity in sortedMembers)
            {
                _turnQueue.Enqueue(entity);
            }
            
        }
        private void EndBattle(BattleState finalState)
        {
            State = finalState;

            foreach (var entity in _allies.Concat(_enemies))
            {
                entity.OnDeath -= CheckBattleStatus;
            }

            //OnBattleStateChanged?.Invoke(State);
        }
    }
}
