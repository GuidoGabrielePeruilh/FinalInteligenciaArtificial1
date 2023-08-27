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

    protected virtual void AddForce(Vector3 force, float speed)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, speed);
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    protected Vector3 CalculateSteering(Vector3 desired, float speed)
    {
        desired.Normalize();
        desired *= speed;
        return Vector3.ClampMagnitude(desired - _velocity, _myEntityData._maxForce);
    }
}
