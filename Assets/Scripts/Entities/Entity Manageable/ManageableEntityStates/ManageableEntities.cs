using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    FSM<ManageableEntityStates> _fsm;
    [SerializeField] Animator myAnimator;

    private void Awake()
    {
        UpdateTargetPosition(transform.position);
        _fsm = new FSM<ManageableEntityStates>();

        IState findPath = new EntityFindPathState(_fsm, this);
        IState attack = new EntityAttackState(_fsm, this,myAnimator,"Attack", _myEntityData.attackCooldown);
        IState idle = new EntityIdleState(_fsm, this);
        _fsm.AddState(ManageableEntityStates.FindPath, findPath);
        _fsm.AddState(ManageableEntityStates.Attack, attack);
        _fsm.AddState(ManageableEntityStates.Idle, idle);

    }

    private void Start()
    {
        _fsm.ChangeState(ManageableEntityStates.FindPath);
    }

    private void Update()
    {
        _fsm.Update();
        Debug.Log(_fsm.CurrentState);
    }

    private void FixedUpdate()
    {
        _fsm.FixedUpdate();
    }

    public Vector3 UpdateTargetPosition(Vector3 targetPosition)
    {
        HasToMove = true;
        return TargetPosition = targetPosition;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _myEntityData.attackRadius);
    }
}
