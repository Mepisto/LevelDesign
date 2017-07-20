using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    public interface ILdSpawnWave
    {
        bool IsActive { get; }

        void StartWave();

        void OnFixedUpdate();
    }

    [Serializable]
    public class LdSpawnWave : ILdSpawnWave
    {
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

        public void OnFixedUpdate()
        {
            if (m_isActive)
            {
                Debug.LogError( string.Format("LdSpawnWave({0}) => OnFixedUpdate", this.WaveId));
            }
        }

        public void StartWave()
        {
            var tr = SpawnPoints[0].GetSpawnTransform();
            var rot = Quaternion.LookRotation(tr.Direction);
            var enemy = Resources.Load("Enemy");

            Global.InstantiateEnemy(enemy, tr.Position, rot);

            m_isActive = true;
        }

        #endregion "ILdSpawner"

        [SerializeField]
        private List<LdSpawnPoint> SpawnPoints = new List<LdSpawnPoint>();

        [SerializeField]
        private uint WaveId;

        public LdSpawnWave()
        {
            m_isActive = false;
        }
    }
}
