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
            Debug.Log("On Enter Follower MoveState");
        }

        public void OnExit()
        {

        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {

            Vector3 dir;

            if (_entity.IsCloseFromLeader())
            {
                if (_leaderToFollow.Velocity.sqrMagnitude < 0.1f)
                {
                    dir = _entity.Separation() * FollowersManager.Instance.SeparationWeight;
                }
                else
                {
                    dir = _entity.Arrive(_leaderToFollow.gameObject) + _entity.SeparationForce;
                }

                _entity.FlockingMove(dir);
            }
            else
            {
                _fsm.ChangeState(FollowersEntitiesStates.Seek);
                return;
            }
        }

    }
}
