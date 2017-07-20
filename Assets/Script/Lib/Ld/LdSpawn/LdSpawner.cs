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

    public interface ILdSpawner
    {
        void Spawn();        
    }

    public class LdSpawner : LevelDesignBase, ILdSpawner
    {
        [SerializeField]
        private List<LdSpawnPoint> SpawnPoints = new List<LdSpawnPoint>();

        [SerializeField]
        private List<LdSpawnWave> SpawnWaves = new List<LdSpawnWave>();

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
        }

        #endregion "ILevelDesign"

        protected override void Awake()
        {
            SpawnPoints.Add(new LdSpawnPoint());
            SpawnWaves.Add(new LdSpawnWave());
        }

        protected override void Start()
        {
        }

        protected override void FixedUpdate()
        {
            for (int i = 0; i < SpawnWaves.Count; ++i)
            {
                SpawnWaves[0].OnFixedUpdate();
            }
        }

        public void Spawn()
        {
            Debug.LogError("Spawn : " + this.gameObject.name);

            var tr = SpawnPoints[0].GetSpawnTransform();
            var rot = Quaternion.LookRotation(tr.Direction);
            var enemy = Resources.Load("Enemy");
            var enemyGO = (GameObject)Instantiate(enemy, tr.Position, rot);
        }
    }
}
