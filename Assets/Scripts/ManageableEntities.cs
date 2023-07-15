using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    private Vector3 _direction;
    private Vector3 _targetPosition;

    private void Start()
    {
        UpdateTargetPosition(this.transform.position);
    }

    private void Update()
    {
        _direction = _targetPosition - transform.position;
        _direction.y = 0;
        if (_myEntityData.distanceToLowSpeed * _myEntityData.distanceToLowSpeed >= _direction.magnitude)
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.fixedDeltaTime * _myEntityData._maxForce * _myEntityData._timeToStop);
            transform.position += _velocity * Time.fixedDeltaTime;
        }
        else
        {
            AddForce(_direction);
        }

    }

    public Vector3 UpdateTargetPosition(Vector3 targetPosition)
    {
        return _targetPosition = targetPosition;
    }
}
