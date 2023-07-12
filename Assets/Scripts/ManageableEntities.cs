using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    private Vector3 _movement;

    private void FixedUpdate()
    {
        if (_movement.magnitude <= 0.1f)
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.fixedDeltaTime * _myEntityData._maxForce * _myEntityData._timeToStop);
            transform.position += _velocity * Time.fixedDeltaTime;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        AddForce(direction);
    }
}
