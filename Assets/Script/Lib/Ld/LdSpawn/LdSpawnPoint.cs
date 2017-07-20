using System;
using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    public interface ILdSpawnPoint
    {

        LdSpawnTransform GetSpawnTransform();
    }

    [Serializable]
    public class LdSpawnPoint : ILdSpawnPoint
    {
        public static readonly float s_fPI = Mathf.PI;
        public static readonly float s_fDegreeToRadian = s_fPI / 180.0f;

        [SerializeField]
        private float LocalRadian;

        [SerializeField]
        private Vector3 Position;

        public LdSpawnPoint()
        {
        }

        public LdSpawnTransform GetSpawnTransform()
        {
            var fRadian = this.LocalRadian * s_fDegreeToRadian;
            var direction = new Vector3(-Mathf.Sin(fRadian), 0.0f, -Mathf.Cos(fRadian));

            var tr = new LdSpawnTransform(this.Position, direction, Vector3.zero);

            return tr;
        }
    }
}
