using IA_I.EntityNS.Follower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.StatesBehaviour
{
    public class FollowerIdleState : IState
    {
        FSM<FollowersEntitiesStates> _fsm;
        FollowersEntities _entity;

        public FollowerIdleState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }
        public void OnEnter()
        {
            Debug.Log($"{_entity.gameObject.name} Enter Idle");

        }

        public void OnExit()
        {
        }

        public void OnLateUpdate()
        {
            _entity.FOV();
        }

        public void OnUpdate()
        {
            if (_entity.HaveTargetToAttack())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Attack);
                return;
            }

            if (!_entity.IsCloseFromLeader() && !_entity.HasLowLife)
            {
                _fsm.ChangeState(FollowersEntitiesStates.Seek);
                return;
            }

            _entity.Stop();

        }
    }
}
