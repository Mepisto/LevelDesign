using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    public interface ILdSpawnWave
    {
        void StartWave();

        void EndWave();

        void OnFixedUpdate(float delta);

        void OnLateFixedUpdate(float delta);

        bool IsValidNextWaveCondition();

        bool IsFinishedCurrentWave();
    }    

    [Serializable]
    public class LdSpawnWave : ILdSpawnWave
    {
        #region "Class Enemy"

        [Serializable]
        public class Enemy
        {
            public enum eSpawnFlag
            {
                Exact = 0,          // Spawn at one of the spawn points exact location
                InRadius = 1,       // Spawn at one of the spawn points in area defined by spawnRadius
                AwayInRadius = 2,   // Spawn at one of the spawn points but spawnRadius away from the player
            }

            public int enemyCount;

            public int npcID;

            public string npcName;

            public eSpawnFlag spawnFlag;

            public float spawnRadius;

            public string startState = string.Empty;

            public int explicitSpawnPoint;

            public void SpawnEnemy()
            {
                if (0 < enemyCount)
                    --this.enemyCount;
            }


#if UNITY_EDITOR
            public void InitializeByInsertArrayElement()
            {
                enemyCount = 1;
                npcID = 0;
                npcName = string.Empty;
                spawnFlag = eSpawnFlag.Exact;
                startState = string.Empty;
                explicitSpawnPoint = -1;
            }
#endif
        }

        #endregion "Class Enemy"

        #region "SerializeField"
        
        [SerializeField]
        protected List<Enemy> EnemyList = new List<Enemy>();
        
        [SerializeField]
        protected List<LdSpawnPoint> SpawnPoints = new List<LdSpawnPoint>();

        [SerializeField]
        public LdWaveCondition WaveCondition;

        #endregion "SerializeField"

        #region "NonSerialized"

        [NonSerialized]
        protected int m_enemyListIndex = 0;

        [NonSerialized]
        protected float m_spendSpawnDelayTime = 0f;

        [NonSerialized]
        protected bool m_isActive = false;

        [NonSerialized]
        protected int m_spawnedCount = 0;

        [NonSerialized]
        protected int m_deathCount = 0;

        #endregion "NonSerialized"

        #region "ILdSpawnWave"

        public void StartWave()
        {
            m_isActive = true;
        }

        public void EndWave()
        {
            Debug.LogError("Go! NextWave");
            m_isActive = false;
            m_spendSpawnDelayTime = 0f;
            m_enemyListIndex = 0;
            m_spawnedCount = 0;
            m_deathCount = 0;
        }

        public void OnFixedUpdate(float delta)
        {
            if (false == m_isActive)
                return;
            
            if (0 < EnemyList[m_enemyListIndex].enemyCount)
            {                
                if (0 < WaveCondition.GetSpawnDelayTime())
                {
                    m_spendSpawnDelayTime += delta;

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

        public void OnLateFixedUpdate(float delta)
        {
            if (m_isActive)
            {
                if (EnemyList[m_enemyListIndex].enemyCount == 0 && m_enemyListIndex < EnemyList.Count - 1)
                {
                    Debug.LogError("NextEnemy List");
                    ++m_enemyListIndex;
                }
            }
        }

        public bool IsValidNextWaveCondition()
        {
            if (m_isActive)
            {
                return CheckNextWaveCondition();
            }

            return false;
        }

        public bool IsFinishedCurrentWave()
        {
            if (m_isActive)
            {
                return CheckFinishedWave();
            }

            return false;
        }

        #endregion "ILdSpawner"

        protected void DelaySpawn()
        {
            if (WaveCondition.GetSpawnDelayTime() <= m_spendSpawnDelayTime)
            {
                // Spawn Delay
                SpawnEnemy();
            }
            else if (0 == m_spawnedCount)
            {
                // Spawn Delay ignore First
                SpawnEnemy();
            }
        }

        protected bool CheckNextWaveCondition()
        {
            switch (WaveCondition.GetNextWaveCondition())
            {
                case eNextWaveCondition.SpawnCount:
                    if (WaveCondition.GetGoalCondition() == m_spawnedCount)//if (EnemyList[m_enemyListIndex].Count == 0 && m_enemyListIndex == EnemyList.Count - 1)                    
                    {
                        return true;
                    }
                    break;
                case eNextWaveCondition.DeathCount:
                    if (WaveCondition.GetGoalCondition() == m_deathCount)
                    {
                        return true;
                    }
                    break;
                case eNextWaveCondition.KillThemAll:
                    if (m_spawnedCount == m_deathCount)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        protected bool CheckFinishedWave()
        {
            if (m_enemyListIndex == EnemyList.Count - 1)
            {
                if (EnemyList[m_enemyListIndex].enemyCount == 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected void SpawnEnemy()
        {
            var tr = SpawnPoints[0].GetSpawnTransform();
            var rot = Quaternion.LookRotation(tr.Direction);
            var enemy = Resources.Load("Enemy");

            Global.InstantiateEnemy(enemy, tr.Position, rot);
            EnemyList[m_enemyListIndex].SpawnEnemy();
            m_spendSpawnDelayTime = 0f;
            m_spawnedCount++;
        }
    }
}
