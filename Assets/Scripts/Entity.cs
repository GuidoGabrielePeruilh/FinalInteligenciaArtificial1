using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityDataSO _myEntityData;
    protected Vector3 _velocity;

    public void Initialize(EntityDataSO entityData)
    {
        _myEntityData = entityData;
    }
    public virtual void AddForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, _myEntityData.speed);
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }
}
