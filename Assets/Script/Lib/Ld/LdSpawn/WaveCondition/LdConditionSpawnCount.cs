using System;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    [Serializable]
    public class LdWaveConditionSpawnCount : LdWaveCondition
    {
        public LdWaveConditionSpawnCount()
        {
            NextWaveCondition = eNextWaveCondition.SpawnCount;
        }

        public override eNextWaveCondition GetNextWaveCondition()
        {
            return NextWaveCondition;
        }

        public override int GetSpawnDelayTime()
        {
            return SpawnDelayTime;
        }

        public override int GetGoalCondition()
        {
            return ConditionGoalCount;
        }
    }
}
