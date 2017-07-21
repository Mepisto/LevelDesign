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
    
    
    /*public class LdSpawnWave_KillBoss : LdSpawnWave
    //{
    //}
    */

    [Serializable]
    public abstract class LdSpawnWave : ILdSpawnWave
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

            public int Count;

            public int npcID = 0;

            public string npcName = string.Empty;

            public eSpawnFlag spawnFlag;

            public float spawnRadius;

            public string spawnAnimationOverride = string.Empty;

            public string startState = string.Empty;

            public int explicitSpawnPoint = -1;

            public void SpawnEnemy()
            {
                if (0 < Count)
                    --this.Count;
            }
        }

        #endregion "Class Enemy"

        #region "SerializeField"

        [SerializeField]
        protected eNextWaveCondition NextWaveCondition;

        [SerializeField]
        protected int SpawnDelayTime;

        [SerializeField]
        protected List<Enemy> EnemyList = new List<Enemy>();
        
        [SerializeField]
        protected List<LdSpawnPoint> SpawnPoints = new List<LdSpawnPoint>();

        #endregion "SerializeField"

        #region "NonSerialized"

        [NonSerialized]
        protected int m_enemyListIndex = 0;

        [NonSerialized]
        protected float m_spendTime = 0f;

        [NonSerialized]
        protected bool m_isActive = false;

        [NonSerialized]
        protected int m_spawnedCount = 0;

        #endregion "NonSerialized"

        #region "ILdSpawnWave"

        public abstract void StartWave();
        public abstract void EndWave();
        public abstract void OnFixedUpdate(float delta);
        public abstract void OnLateFixedUpdate(float delta);
        public abstract bool IsValidNextWaveCondition();

        /*public void StartWave()
        //{
        //    m_isActive = true;
        //}
        */

        /*public void EndWave()
        //{
        //    Debug.LogError("Go! NextWave");
        //    m_isActive = false;
        //    m_spendTime = 0f;
        //    m_enemyListIndex = 0;
        //    m_spawnedCount = 0;
        //}
        */

        /*public void OnFixedUpdate(float delta)
        //{
        //    if (false == m_isActive)
        //        return;

        //    if (0 < EnemyList[m_enemyListIndex].Count)
        //    {
        //        if (0 < SpawnDelayTime)
        //        {
        //            m_spendTime += delta;

        //            // Spawn Delay
        //            DelaySpawn();
        //        }
        //        else
        //        {
        //            // Spawn Direct
        //            SpawnEnemy();
        //        }
        //    }
        //}        
        */

        /*public void OnLateFixedUpdate(float delta)
        //{
        //    if (m_isActive)
        //    {
        //        if (EnemyList[m_enemyListIndex].Count == 0 && m_enemyListIndex < EnemyList.Count - 1)
        //        {
        //            Debug.LogError("NextEnemy List");
        //            ++m_enemyListIndex;
        //        }
        //    }
        //}
        */

        /*public bool IsValidNextWaveCondition()
        //{
        //    if (m_isActive)
        //    {
        //        return CheckNextWaveCondition();
        //    }

        //    return false;
        //}
        */

        #endregion "ILdSpawner"

        protected void DelaySpawn()
        {
            if (SpawnDelayTime <= m_spendTime)
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
            switch (NextWaveCondition)
            {
                case eNextWaveCondition.SpawnCount:
                    if (EnemyList[m_enemyListIndex].Count == 0 && m_enemyListIndex == EnemyList.Count - 1)
                    {
                        return true;
                    }
                    break;
                case eNextWaveCondition.KillThemAll:
                    break;
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
            m_spendTime = 0f;
            m_spawnedCount++;
        }

#if UNITY_EDITOR
        public abstract void InitializeByInsertArrayElement();
#endif
    }
}
