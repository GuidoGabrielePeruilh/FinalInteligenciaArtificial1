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
            if (_entity.HasToRunAway)
            {
                _fsm.ChangeState(FollowersEntitiesStates.RunAway);
                return;
            }

            if (_entity.HaveTargetToAttack())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Attack);
                return;
            }

            if (_entity.HasToMoveInPath)
            {
                _fsm.ChangeState(FollowersEntitiesStates.Move);
                return;
            }

            _entity.FlockingMove(_entity.Arrive(_entity.LeaderToFollow.gameObject));

        }
    }
}
