using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    FSM<ManageableEntityStates> _fsm;
    public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        UpdateTargetPosition(transform.position);
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
}
