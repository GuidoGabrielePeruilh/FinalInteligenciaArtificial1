using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityDataSO _myEntityData;
    [SerializeField] private int collisionLayer;

    protected Vector3 _velocity;

    public void Initialize(EntityDataSO entityData)
    {
        _myEntityData = entityData;
    }
}
