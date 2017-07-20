using System;

namespace Orca.Contents.LevelDesign
{
    public interface ILdSpawnWave
    {
        bool IsActive { get; }

        void OnFixedUpdate();

    }

    [System.Serializable]
    public class LdSpawnWave : ILdSpawnWave
    {
        #region "ILdSpawner"

        private bool m_isActive;
        public bool IsActive
        {
            get
            {
                return m_isActive;
            }
        }

        public void Spawn()
        {
            throw new NotImplementedException();
        }

        #endregion "ILdSpawner"

        public LdSpawnWave()
        {
            m_isActive = false;
        }

        public void OnFixedUpdate()
        {
            if (m_isActive)
            {

            }
        }
    }
}
