using IA_I.EntityNS.Follower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.StatesBehaviour
{
    public class FollowerAttackState : IState
    {

        FSM<FollowersEntitiesStates> _fsm;
        FollowersEntities _entity;
        Animator _myAnimator;
        string _attackAnimationName;
        float _attackCooldown;
        float _timer;
        GameObject _target;

        public FollowerAttackState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {
            if (_entity.HasToMoveInPath)
            {
                /* PARA QUE EL ENTITY SE VAYA EN CASO DE TENER QUE ESCAPAR
                if (_myEntity.HasLowLife)
                {
                    _myEntity.UpdateTargetPosition(_myEntity.GetRandomNodeToRun().transform.position);
                }
                */
                _fsm.ChangeState(FollowersEntitiesStates.Move);
                return;
            }

            if (!_entity.HaveTargetToAttack())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Idle);
                return;
            }
        }
    }
}
