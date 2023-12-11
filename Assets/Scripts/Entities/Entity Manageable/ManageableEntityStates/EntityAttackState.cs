using IA_I.EntityNS.Manegeable;
using UnityEngine;
using System.Linq;

namespace IA_I.FSM.StatesBehaviour
{
    public class EntityAttackState : IState
    {
        FSM<ManageableEntityStates> _fsm;
        ManageableEntities _myEntity;
        Animator _myAnimator;
        string _attackAnimationName;
        float _attackCooldown;
        float _timer;
        Transform _target;


        public EntityAttackState(FSM<ManageableEntityStates> fsm, ManageableEntities myEntity, Animator myAnimator, string attackAnimationName, float attackCooldown)
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
        }

        public void OnExit()
        {
            _target = null;
            _timer = 0;
        }

        public void OnLateUpdate()
        {
            _myEntity.FOV();
        }

        public void OnUpdate()
        {

            if (_target == null || !_myEntity.HaveTargetToAttack())
            {
                _fsm.ChangeState(ManageableEntityStates.Idle);
                return;
            }

            Attack();

            if (_myEntity.HasToMove)
            {
                _fsm.ChangeState(ManageableEntityStates.Move);
                return;
            }
        }

        private void Attack()
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
}

