using IA_I.EntityNS.Follower;
using IA_I.EntityNS.Manegeable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.StatesBehaviour
{
    public class FollowerRunAwayState : IState
    {
        FSM<FollowersEntitiesStates> _fsm;
        Node _startingNode, _goalNode;
        Stack<Node> _pathToFollow;
        PathFinding _pathfinding;
        Vector3 _targetPosition;
        FollowersEntities _entity;

        public FollowerRunAwayState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }
        public void OnEnter()
        {
            var randomNode = _entity.GetRandomNodeToRun();
            if (randomNode != null)
                _targetPosition = randomNode.transform.position;
            else
                _targetPosition = _entity.transform.position;

            UpdatePath();
        }

        public void OnExit()
        {
            _startingNode = null;
            _goalNode = null;
        }

        public void OnLateUpdate()
        {
        }

        public void OnUpdate()
        {
            _entity.FollowPath(_pathToFollow);

            if (_entity.HasArriveToDestiny)
                _fsm.ChangeState(FollowersEntitiesStates.Idle);

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
