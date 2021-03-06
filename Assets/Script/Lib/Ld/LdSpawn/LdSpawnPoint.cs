﻿using System;
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
        [Flags]
        public enum eSpawnFlags
        {
            IsDisabled = 0x00000001,
            IsBossPoint = 0x00000002,
            IgnoreHeroOnSpawn = 0x00000004,   // Spawn in the direction defined with IsRightAligned
                                              // Do not spawn facing a hero!
            IsRightAligned = 0x00000008,   // Defines the direction NPCs are facing when spawning
                                           // Default is to the left (-x)
                                           // This is only used if there is no hero in the level
            LocalSpace = 0x00000010,   // Spawn point is considered to be in local space of the SpawnGroup
        }

        public static readonly float s_fPI = Mathf.PI;
        public static readonly float s_fDegreeToRadian = s_fPI / 180.0f;

        [SerializeField]
        private int LocalRadian;

        [SerializeField]
        private Vector3 Position;

        [SerializeField]
        private eSpawnFlags spawnFlags = 0;

        public LdSpawnPoint()
        {
        }

        public bool GetFlag(eSpawnFlags flag)
        {
            return (spawnFlags & flag) == flag;
        }

        public void SetFlag(eSpawnFlags flag, bool set)
        {
            if (set)
            {
                spawnFlags |= flag;
            }
            else
            {
                spawnFlags &= ~flag;
            }
        }

        public LdSpawnTransform GetSpawnTransform()
        {
            var fRadian = this.LocalRadian * s_fDegreeToRadian;
            var direction = new Vector3(-Mathf.Sin(fRadian), 0.0f, -Mathf.Cos(fRadian));

            var tr = new LdSpawnTransform(this.Position, direction, Vector3.zero);

            return tr;
        }

#if UNITY_EDITOR
        public void InitializeByInsertArrayElement()
        {
            SetFlag(eSpawnFlags.LocalSpace, true);
        }

        public void SetPosition(Vector3 worldPosition, Transform transform)
        {
            if (GetFlag(eSpawnFlags.LocalSpace))
            {
                Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
                Position = localPosition;
            }
            else
            {
                Position = worldPosition;
            }
        }
#endif
    }
}
