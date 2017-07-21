using System;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    [Serializable]
    public class LdConditionSpawnCount : LdSpawnWave
    {  
        #region "ILdSpawnWave"

        public override void StartWave()
        {
            m_isActive = true;
        }

        public override void EndWave()
        {
            Debug.LogError("Go! NextWave");
            m_isActive = false;
            m_spendTime = 0f;
            m_enemyListIndex = 0;
            m_spawnedCount = 0;
        }

        public override void OnFixedUpdate(float delta)
        {
            if (false == m_isActive)
                return;

            if (0 < EnemyList[m_enemyListIndex].Count)
            {
                if (0 < SpawnDelayTime)
                {
                    m_spendTime += delta;

                    // Spawn Delay
                    DelaySpawn();
                }
                else
                {
                    // Spawn Direct
                    SpawnEnemy();
                }
            }
        }

        public override void OnLateFixedUpdate(float delta)
        {
            if (m_isActive)
            {
                if (EnemyList[m_enemyListIndex].Count == 0 && m_enemyListIndex < EnemyList.Count - 1)
                {
                    Debug.LogError("NextEnemy List");
                    ++m_enemyListIndex;
                }
            }
        }

        public override bool IsValidNextWaveCondition()
        {
            if (m_isActive)
            {
                return CheckNextWaveCondition();
            }

            return false;
        }

        #endregion "ILdSpawner"

#if UNITY_EDITOR
        public override void InitializeByInsertArrayElement()
        {
            this.NextWaveCondition = eNextWaveCondition.SpawnCount;
        }
#endif
    }
}
