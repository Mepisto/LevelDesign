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
    }

    [Serializable]
    public class LdSpawnWave : ILdSpawnWave
    {
        #region "Class Enemy"

        [Serializable]
        public class Enemy
        {
            public int Count;

            public void SpawnEnemy()
            {
                if (0 < Count)
                    --this.Count;
            }
        }

        #endregion "Class Enemy"

        #region "SerializeField"

        [SerializeField]
        private eNextWaveCondition NextWaveCondition;

        [SerializeField]
        private int SpawnDelayTime;

        [SerializeField]
        private List<Enemy> EnemyList = new List<Enemy>();
        
        [SerializeField]
        private List<LdSpawnPoint> SpawnPoints = new List<LdSpawnPoint>();

        #endregion "SerializeField"

        #region "NonSerialized"

        [NonSerialized]
        private int m_enemyWaveId;

        [NonSerialized]
        private float m_spendTime;

        [NonSerialized]
        private bool m_isActive;

        [NonSerialized]
        private int m_spawnedCount = 0;

        #endregion "NonSerialized"

        public LdSpawnWave()
        {
            m_isActive = false;
        }

        #region "ILdSpawnWave"        
        
        public void StartWave()
        {
            m_isActive = true;
        }

        public void EndWave()
        {
            Debug.LogError("Go! NextWave");
            m_isActive = false;
            m_spendTime = 0f;
            m_enemyWaveId = 0;
            m_spawnedCount = 0;
        }

        public void OnFixedUpdate(float delta)
        {
            if (false == m_isActive)
                return;

            m_spendTime += delta;

            if (0 < EnemyList[m_enemyWaveId].Count)
            {
                if (0 < SpawnDelayTime)
                {
                    if (SpawnDelayTime <= m_spendTime)
                    {
                        SpawnEnemy();
                    }
                    else if (0 == m_spawnedCount)
                    {
                        SpawnEnemy();
                    }
                }
                else
                {
                    SpawnEnemy();
                }
            }
        }

        public void OnLateFixedUpdate(float delta)
        {
            if (m_isActive)
            {
                if (EnemyList[m_enemyWaveId].Count == 0 && m_enemyWaveId < EnemyList.Count - 1)
                {
                    Debug.LogError("NextEnemy List");
                    ++m_enemyWaveId;
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

        #endregion "ILdSpawner"

        private bool CheckNextWaveCondition()
        {
            switch (NextWaveCondition)
            {
                case eNextWaveCondition.SpawnCount:
                    if (EnemyList[m_enemyWaveId].Count == 0 && m_enemyWaveId == EnemyList.Count - 1)
                    {
                        return true;
                    }
                    break;
                case eNextWaveCondition.KillThemAll:
                    break;
            }

            return false;
        }
        
        private void SpawnEnemy()
        {
            var tr = SpawnPoints[0].GetSpawnTransform();
            var rot = Quaternion.LookRotation(tr.Direction);
            var enemy = Resources.Load("Enemy");

            Global.InstantiateEnemy(enemy, tr.Position, rot);
            EnemyList[m_enemyWaveId].SpawnEnemy();
            m_spendTime = 0f;
            m_spawnedCount++;
        }        
    }
}
