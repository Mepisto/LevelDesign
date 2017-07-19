using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelDesign
{
    void Init();

    bool IsValidID(uint id);

    void Active(bool isActive);
}

public abstract class LevelDesignBase : MonoBehaviour, ILevelDesign
{
    [SerializeField]
    protected uint Id;

    [SerializeField]
    protected bool IsActive;

    [SerializeField]
    protected float LocalRadian;

    [SerializeField]
    protected Vector3 Position;

    protected eLevelDesignCategory eLdCategory;

    protected eLevelDesignType LdType;
    

    public abstract void Init();

    public abstract bool IsValidID(uint id);

    public abstract void Active(bool isActive);

    protected virtual void Start()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {        
    }

    protected virtual void OnTriggerExit(Collider other)
    {
    }
}
