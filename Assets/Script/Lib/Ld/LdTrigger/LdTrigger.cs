﻿using UnityEngine;

namespace Orca.Contents.LevelDesign
{
    public class LdTrigger : LevelDesignBase
    {
        #region "ILevelDesign"

        public override void Init()
        {
            this.Position = Vector3.zero;
            this.LocalRadian = 0f;
            this.IsActive = true;
            this.eLdCategory = eLevelDesignCategory.Trigger;
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

        }

        protected override void Start()
        {

        }

        protected override void FixedUpdate()
        {
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (IsActive)
            {
                TriggerEnterMsg msg = new TriggerEnterMsg();
                msg.EnterInfo = new TriggerEnterInfo(Id);
                Server.RequestMsg(msg);
                //Global.PostMessage(msg);
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            //TriggerExitMsg msg = new TriggerExitMsg();
            //msg.ExitInfo = new TriggerExitInfo();

            //Server.RequestMsg(msg);
        }
    }
}
