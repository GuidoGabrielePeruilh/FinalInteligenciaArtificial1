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
        UpdateTargetPosition(this.transform.position);
        _fsm = new FSM<ManageableEntityStates>();

        IState findPath = new EntityFindPathState(_fsm, this);
        _fsm.AddState(ManageableEntityStates.FindPath, findPath);

    }

    private void Start()
    {
        _fsm.ChangeState(ManageableEntityStates.FindPath);
    }

    private void Update()
    {
        _fsm.Update();
    }

    private void FixedUpdate()
    {
        _fsm.FixedUpdate();
    }

    public Vector3 UpdateTargetPosition(Vector3 targetPosition)
    {
        return TargetPosition = targetPosition;
    }

    public void FollowPath(Stack<Node> pathToFollow)
    {
        if (pathToFollow.Count == 0) return;

        Vector3 nextPos = pathToFollow.Peek().transform.position;
        Vector3 dir = nextPos - transform.position;
        dir.y = 0;

        AddForce(CalculateSteering(dir, _myEntityData.speed), _myEntityData.speed);


        if (dir.sqrMagnitude < _myEntityData.distanceToLowSpeed * _myEntityData.distanceToLowSpeed)
        {
            pathToFollow.Pop();
        }
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
