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
        GameObject _target;


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

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {
            if (_myEntity.HasToMove)
            {
                /* PARA QUE EL ENTITY SE VAYA EN CASO DE TENER QUE ESCAPAR
                if (_myEntity.HasLowLife)
                {
                    _myEntity.UpdateTargetPosition(_myEntity.GetRandomNodeToRun().transform.position);
                }
                */
                _fsm.ChangeState(ManageableEntityStates.FindPath);
                return;
            }

            if (!_myEntity.HaveTargetToAttack())
            {
                _fsm.ChangeState(ManageableEntityStates.Idle);
                return;
            }

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

