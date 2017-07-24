using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    [Serializable]
    public class LdWaveConditionKillThemAll : LdWaveCondition
    {
        public LdWaveConditionKillThemAll()
        {
            NextWaveCondition = eNextWaveCondition.KillThemAll;
        }

        public override eNextWaveCondition GetNextWaveCondition()
        {
            return NextWaveCondition;
        }

        public override int GetSpawnDelayTime()
        {
            return SpawnDelayTime;
        }
    }
}
