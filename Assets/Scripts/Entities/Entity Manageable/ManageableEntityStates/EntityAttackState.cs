using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackState : IState
{
    FSM<ManageableEntityStates> _fsm;
    ManageableEntities _myEntity;
    Animator _myAnimator;
    string _attackAnimationName;
    float _attackCooldown;
    float _timer;
    GameObject _target;
    Vector3 _initialPosition;


    public EntityAttackState(FSM<ManageableEntityStates> fsm, ManageableEntities myEntity, Animator myAnimator,string attackAnimationName,  float attackCooldown)
    {
        _fsm = fsm;
        _myEntity = myEntity;
        _myAnimator = myAnimator;
        _attackAnimationName = attackAnimationName;
        _attackCooldown = attackCooldown;
    }

    public void OnEnter()
    {
        _timer = _attackCooldown;
        _target = _myEntity.AttackTarget;
        _initialPosition = _myEntity.transform.position;
        _myEntity.UpdateTargetPosition(_initialPosition);
    }

    public void OnExit()
    {
        _target = null;
        _timer = 0;
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        _timer += Time.deltaTime;

        if (!_myEntity.HaveTargetToAttack())
        {
            _fsm.ChangeState(ManageableEntityStates.FindPath);
            _target = _myEntity.AttackTarget;
            return;
        }

        if (_initialPosition != _myEntity.TargetPosition)
        {
            _fsm.ChangeState(ManageableEntityStates.FindPath);
            return;
        }

        Vector3 lookDirection = (_target.transform.position - _myEntity.transform.position).normalized;
        _myEntity.transform.forward = lookDirection;

        if (_timer >= _attackCooldown)
        {
            _myAnimator.SetTrigger(_attackAnimationName);
            _timer = 0;
        }
        
    }
}
