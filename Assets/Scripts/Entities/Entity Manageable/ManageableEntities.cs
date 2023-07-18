using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    FSM<ManageableEntityStates> _fsm;
    public Vector3 TargetPosition { get; private set; }
    public Vector3 ActualPosition { get; private set; }


    private void Awake()
    {

        _fsm = new FSM<ManageableEntityStates>();


        IState findPath = new EntityFindPathState(_fsm, this);
        _fsm.AddState(ManageableEntityStates.FindPath, findPath);

        _fsm.ChangeState(ManageableEntityStates.FindPath);

    }

    private void Start()
    {
        UpdateTargetPosition(this.transform.position);
    }

    private void Update()
    {
        ActualPosition = this.transform.position;
        //_direction = _targetPosition - transform.position;
        //_direction.y = 0;
        //if (_myEntityData.distanceToLowSpeed * _myEntityData.distanceToLowSpeed >= _direction.magnitude)
        //{
        //    _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.fixedDeltaTime * _myEntityData._maxForce * _myEntityData._timeToStop);
        //    transform.position += _velocity * Time.fixedDeltaTime;
        //}
        //else
        //{
        //    AddForce(_direction);
        //}
    }

    public Vector3 UpdateTargetPosition(Vector3 targetPosition)
    {
        return TargetPosition = targetPosition;
    }
}
