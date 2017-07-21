using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    #region "Struct LdSpawnTransform"

    public struct LdSpawnTransform
    {
        private Vector3 m_position;
        private Vector3 m_direction;
        private Vector3 m_scale;

        public LdSpawnTransform(Vector3 position, Vector3 direction, Vector3 scale)
        {
            m_position = position;
            m_direction = direction;
            m_scale = scale;
        }

        public Vector3 Position { get { return m_position; } }

        public Vector3 Direction { get { return m_direction; } }

        public Vector3 Scale { get { return m_scale; } }
    }

    #endregion "Struct LdSpawnTransform"

    public class LdSpawner : LevelDesignBase
    {
        [SerializeField]
        private List<LdConditionSpawnCount> SpawnCountWaves = new List<LdConditionSpawnCount>();

        [SerializeField]
        private List<LdConditionDeathCount> DeathCountWaves = new List<LdConditionDeathCount>();

        [SerializeField]
        private List<LdConditionKillThemAll> KillThemAllWaves = new List<LdConditionKillThemAll>();
        
        [NonSerialized]
        private List<ILdSpawnWave> m_spawnWaves = new List<ILdSpawnWave>();

        [NonSerialized]
        private int m_currentWave = 0;

        #region "ILevelDesign"

        public override void LdInit()
        {
            this.IsActive = false;
            this.eLdCategory = eLevelDesignCategory.Spawner;

            SpawnCountWaves.Add(new LdConditionSpawnCount());

            m_spawnWaves.AddRange(SpawnCountWaves.ToArray());
            m_spawnWaves.AddRange(DeathCountWaves.ToArray());
            m_spawnWaves.AddRange(KillThemAllWaves.ToArray());
        }

        public override bool IsValidID(uint id)
        {
            return this.Id == id;
        }

        public override void Active(bool isActive)
        {
            this.IsActive = isActive;

            m_spawnWaves[m_currentWave].StartWave();
        }

        #endregion "ILevelDesign"

        protected override void Awake()
        {
        }

        protected override void Start()
        {
        }

        protected override void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            for (int i = 0; i < m_spawnWaves.Count; ++i)
            {
                m_spawnWaves[i].OnFixedUpdate(delta);
            }


            for (int i = 0; i < m_spawnWaves.Count; ++i)
            {
                m_spawnWaves[i].OnLateFixedUpdate(delta);

                if (m_spawnWaves[i].IsValidNextWaveCondition())
                {
                    m_spawnWaves[m_currentWave++].EndWave();

                    if (m_currentWave < m_spawnWaves.Count)
                    {
                        m_spawnWaves[m_currentWave].StartWave();
                    }
                }
            }
        }
    }
}
