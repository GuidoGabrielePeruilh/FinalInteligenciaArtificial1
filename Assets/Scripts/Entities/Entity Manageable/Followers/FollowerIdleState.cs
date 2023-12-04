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
            Debug.Log("On Enter Followe IdleState");
        }

        public void OnExit()
        {
            Debug.Log("On Exit follower Idle State");
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {
            if (_entity.HaveTargetToAttack())
                _fsm.ChangeState(FollowersEntitiesStates.Attack);

            if (_entity.HasToMoveInPath)
                _fsm.ChangeState(FollowersEntitiesStates.Move);

            _entity.FlockingMove(_entity.Arrive(_entity.LeaderToFollow.gameObject));

        }
    }
}
