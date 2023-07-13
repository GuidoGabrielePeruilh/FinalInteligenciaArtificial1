using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    private Vector3 _direction;
    private Vector3 _targetPosition;

    private void Update()
    {
        _direction = _targetPosition - transform.position;
        _direction.y = 0;
        AddForce(_direction);
    }

    private void FixedUpdate()
    {

        if (_direction.magnitude <= 0.1f)
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.fixedDeltaTime * _myEntityData._maxForce * _myEntityData._timeToStop);
            transform.position += _velocity * Time.fixedDeltaTime;
        }
    }

    public Vector3 UpdateTargetPosition(Vector3 targetPosition)
    {
        return _targetPosition = targetPosition;
    }
}
