using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackState : IState
{
    FSM<ManageableEntityStates> _fsm;
    ManageableEntities _myEntity;
    GameObject _target;
    Animator _myAnimator;
    string _attackAnimationName;
    float _attackCooldown;
    float _timer;

    public EntityAttackState(FSM<ManageableEntityStates> fsm, ManageableEntities myEntity, GameObject target, Animator myAnimator,string attackAnimationName,  float attackCooldown)
    {
        _fsm = fsm;
        _myEntity = myEntity;
        _target = target;
        _myAnimator = myAnimator;
        _attackAnimationName = attackAnimationName;
        _attackCooldown = attackCooldown;
    }

    public void OnEnter()
    {
        _timer = _attackCooldown;
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        _timer += Time.deltaTime;
        Vector3 lookDirection = (_target.transform.position - _myEntity.transform.position).normalized;
        _myEntity.transform.forward = lookDirection;

        if (_timer >= _attackCooldown)
        {
            _myAnimator.SetTrigger(_attackAnimationName);
            _timer = 0;
        }
        
    }
}
