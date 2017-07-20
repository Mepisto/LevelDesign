using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    public interface ILdSpawnWave
    {
        bool IsActive { get; }

        void StartWave();

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

        #region "Variables"

        [SerializeField]
        private int EnemyWaveId;

        [SerializeField]
        private eNextWaveCondition NextWaveCondition;

        [SerializeField]
        private int SpawnDelayTime;
        
        [NonSerialized]
        private float SpendTime;

        [SerializeField]
        private List<Enemy> EnemyList = new List<Enemy>();
        
        [SerializeField]
        private List<LdSpawnPoint> SpawnPoints = new List<LdSpawnPoint>();
        
        #endregion "Variables"

        public LdSpawnWave()
        {
            m_isActive = false;
        }

        #region "ILdSpawnWave"

        [NonSerialized]
        private bool m_isActive;
        public bool IsActive
        {
            get
            {
                return m_isActive;
            }
        }

        public void StartWave()
        {

            //GetCurrentWave();
            

            m_isActive = true;
        }
        
        public void OnFixedUpdate(float delta)
        {
            if (m_isActive)
            {
                SpendTime += delta;

                if (0 < EnemyList[EnemyWaveId].Count)
                {
                    if (0 < SpawnDelayTime && SpawnDelayTime < SpendTime)
                    {
                        SpawnEnemy();
                    }
                    else
                    {
                        SpawnEnemy();
                    }
                }
            }
        }

        public void OnLateFixedUpdate(float delta)
        {
            if (m_isActive)
            {
                //Debug.LogError( string.Format("LdSpawnWave({0}) => OnFixedUpdate", this.WaveId));
            }
        }

        public bool IsValidNextWaveCondition()
        {
            if (m_isActive)
            {
                return CheckWaveCondition();
            }

            return false;
        }

        #endregion "ILdSpawner"

        private Enemy GetCurrentEnemy()
        {
            return EnemyList[EnemyWaveId];
        }

        private bool CheckWaveCondition()
        {
            switch (NextWaveCondition)
            {
                case eNextWaveCondition.SpawnCount:
                    {
                        if (EnemyList[EnemyWaveId].Count == 0 && EnemyWaveId < EnemyList.Count - 1)
                        {
                            ++EnemyWaveId;
                            return true;
                        }                        
                    }
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
            EnemyList[EnemyWaveId].SpawnEnemy();
        }
    }
}
