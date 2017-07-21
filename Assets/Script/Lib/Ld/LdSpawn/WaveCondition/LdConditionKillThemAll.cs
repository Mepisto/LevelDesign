using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    [Serializable]
    public class LdConditionKillThemAll : LdSpawnWave
    {
        #region "ILdSpawnWave"

        public override void EndWave()
        {
            throw new NotImplementedException();
        }

        public override bool IsValidNextWaveCondition()
        {
            throw new NotImplementedException();
        }

        public override void OnFixedUpdate(float delta)
        {
            throw new NotImplementedException();
        }

        public override void OnLateFixedUpdate(float delta)
        {
            throw new NotImplementedException();
        }

        public override void StartWave()
        {
            throw new NotImplementedException();
        }

        #endregion "ILdSpawnWave"

#if UNITY_EDITOR
        public override void InitializeByInsertArrayElement()
        {
            this.NextWaveCondition = eNextWaveCondition.KillThemAll;
        }
#endif
    }
}
