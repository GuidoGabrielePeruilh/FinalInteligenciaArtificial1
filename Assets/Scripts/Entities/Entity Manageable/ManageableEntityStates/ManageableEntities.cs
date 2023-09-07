using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageableEntities : Entity
{
    FSM<ManageableEntityStates> _fsm;
    [SerializeField] Animator myAnimator;
    public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        UpdateTargetPosition(transform.position);
        _fsm = new FSM<ManageableEntityStates>();

        IState findPath = new EntityFindPathState(_fsm, this);
        IState attack = new EntityAttackState(_fsm, this,myAnimator,"Attack", _myEntityData.attackCooldown);
        _fsm.AddState(ManageableEntityStates.FindPath, findPath);
        _fsm.AddState(ManageableEntityStates.Attack, attack);

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

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _myEntityData.attackRadius);
    }
}
