using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILdSpawner
{
    void Spawn();

}

public class LdSpawner : LevelDesignBase, ILdSpawner
{
    public static readonly float s_fPI = Mathf.PI;
    public static readonly float s_fDegreeToRadian = s_fPI / 180.0f;


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

    protected override void Start ()
    {
		
	}

    protected override void FixedUpdate()
    {
    }

    public void Spawn()
    {
        Debug.LogError("Spawn");

        var fRadian = this.LocalRadian * s_fDegreeToRadian;
        var direction = new Vector3(-Mathf.Sin(fRadian), 0.0f, -Mathf.Cos(fRadian));
        var rot = Quaternion.LookRotation(direction);

        var enemy = Resources.Load("Enemy");
        var enemyGO = (GameObject)Instantiate(enemy, this.Position, rot);
    }    
}
