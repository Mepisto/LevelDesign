using System;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    public interface ILdWaveCondition
    {
        eNextWaveCondition GetNextWaveCondition();

        int GetSpawnDelayTime();

        int GetGoalCondition();
    }

    [Serializable]
    public class LdWaveCondition : ILdWaveCondition
    {
        [SerializeField]
        protected eNextWaveCondition NextWaveCondition;

        [SerializeField]
        protected int SpawnDelayTime;

        [SerializeField]
        protected int ConditionGoalCount;

        public virtual eNextWaveCondition GetNextWaveCondition() { return NextWaveCondition; }

        public virtual int GetSpawnDelayTime() { return SpawnDelayTime; }

        public virtual int GetGoalCondition() { return ConditionGoalCount; }
    }
}
