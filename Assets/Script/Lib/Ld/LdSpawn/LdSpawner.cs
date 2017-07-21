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
        private List<LdSpawnWave> SpawnWaves = new List<LdSpawnWave>();

        [NonSerialized]
        private int m_currentWave = 0;

        #region "ILevelDesign"

        public override void Init()
        {
            this.IsActive = false;
            this.eLdCategory = eLevelDesignCategory.Spawner;
        }

        public override bool IsValidID(uint id)
        {
            return this.Id == id;
        }

        public override void Active(bool isActive)
        {
            this.IsActive = isActive;

            SpawnWaves[m_currentWave].StartWave();
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
            for (int i = 0; i < SpawnWaves.Count; ++i)
            {
                SpawnWaves[i].OnFixedUpdate(delta);
            }


            for (int i = 0; i < SpawnWaves.Count; ++i)
            {
                SpawnWaves[i].OnLateFixedUpdate(delta);

                if (SpawnWaves[i].IsValidNextWaveCondition())
                {
                    SpawnWaves[m_currentWave++].EndWave();

                    if (m_currentWave < SpawnWaves.Count)
                    {
                        SpawnWaves[m_currentWave].StartWave();
                    }
                }
            }
        }
    }
}
