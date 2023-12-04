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
        ManageableEntities _leader;

        public FollowerSeekState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity, ManageableEntities leader)
        {
            _fsm = fsm;
            _entity = entity;
            _leader = leader;
        }

        public void OnEnter()
        {
            Debug.Log("On Enter Follower Seek State");
            _targetPosition = _entity.TargetPosition;
            UpdatePath();
        }

        public void OnExit()
        {
            Debug.Log("On Exit Follower Seek State");
            _startingNode = null;
            _goalNode = null;
        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {
            if (!_entity.HasToMoveInPath)
            {
                if (_entity.IsCloseFromLeader())
                {
                    _fsm.ChangeState(FollowersEntitiesStates.Idle);
                    return;
                }
                else
                {
                    _fsm.ChangeState(FollowersEntitiesStates.Move);
                    return;
                }
            }
            else
            {
                _entity.FollowPath(_pathToFollow);
                if (_targetPosition == _entity.TargetPosition) return;
                _targetPosition = _entity.TargetPosition;
                UpdatePath();
            }

        }

        private void UpdatePath()
        {
            _startingNode = NodesManager.Instance.SetNode(_entity.transform.position);
            _goalNode = NodesManager.Instance.SetNode(_targetPosition);
            _pathToFollow = new Stack<Node>();
            _pathfinding = new PathFinding();
            _pathToFollow = _pathfinding.AStar(_startingNode, _goalNode);
        }
    }
}
