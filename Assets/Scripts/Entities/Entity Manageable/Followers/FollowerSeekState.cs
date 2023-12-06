using IA_I.EntityNS.Follower;
using IA_I.EntityNS.Manegeable;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.StatesBehaviour
{
    public class FollowerSeekState : IState
    {
        FSM<FollowersEntitiesStates> _fsm;
        Node _startingNode, _goalNode;
        Stack<Node> _pathToFollow;
        PathFinding _pathfinding;
        Vector3 _targetPosition;
        FollowersEntities _entity;

        public FollowerSeekState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }

        public void OnEnter()
        {
            Debug.Log("Enter Seek");
            _targetPosition = _entity.TargetPosition;
            UpdatePath();
        }

        public void OnExit()
        {
            _startingNode = null;
            _goalNode = null;
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

            _entity.FollowPath(_pathToFollow);

            if (_entity.HasArriveToDestiny)
            {
                if (_entity.IsCloseFromLeader())
                    _fsm.ChangeState(FollowersEntitiesStates.Idle);
                else
                    _fsm.ChangeState(FollowersEntitiesStates.Move);

                return;
            }

            if (_targetPosition == _entity.TargetPosition) return;
            _targetPosition = _entity.TargetPosition;
            UpdatePath();
        }

        private void UpdatePath()
        {
            _startingNode = NodesManager.Instance.SetNode(_entity.transform.position);
            _goalNode = NodesManager.Instance.SetNode(_targetPosition);
            _pathToFollow = new Stack<Node>();
            _pathfinding = new PathFinding();
            _pathToFollow = _pathfinding.ThetaStar(_startingNode, _goalNode);
        }
    }
}
