using IA_I.EntityNS.Follower;
using IA_I.EntityNS.Manegeable;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.StatesBehaviour
{
    public class FollowerMoveState : IState
    {
        FSM<FollowersEntitiesStates> _fsm;
        ManageableEntities _leaderToFollow;
        FollowersEntities _entity;

        public FollowerMoveState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity, ManageableEntities leader)
        {
            _fsm = fsm;
            _entity = entity;
            _leaderToFollow = leader;
        }

        public void OnEnter()
        {
            Debug.Log($"{_entity.gameObject.name} Enter Move");

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

            if (_entity.HasLowLife)
            {
                _fsm.ChangeState(FollowersEntitiesStates.RunAway);
                return;
            }

            if (_entity.HaveTargetToAttack())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Attack);
                return;
            }

            if (_entity.HasToMove)
            {
                _fsm.ChangeState(FollowersEntitiesStates.Seek);
                return;
            }


            if (_entity.IsCloseFromLeader())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Idle);
                return;
            }


            Vector3 dir;


            dir = _entity.Arrive(_leaderToFollow.gameObject);


            _entity.FlockingMove(dir);
        }

    }
}
