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

        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {
            if (_entity.HaveTargetToAttack())
                _fsm.ChangeState(FollowersEntitiesStates.Attack);

            if (_entity.HasToMove)
                _fsm.ChangeState(FollowersEntitiesStates.Move);

        }
    }
}
