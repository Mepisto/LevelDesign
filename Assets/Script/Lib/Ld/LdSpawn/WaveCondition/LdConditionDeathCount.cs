using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    [Serializable]
    public class LdWaveConditionDeathCount : LdWaveCondition
    {
        public LdWaveConditionDeathCount()
        {
            NextWaveCondition = eNextWaveCondition.DeathCount;
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
