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
            Debug.Log("Enter Run Away");
            _targetPosition = _entity.GetRandomNodeToRun().transform.position;
            UpdatePath();
        }

        public void OnExit()
        {
            _startingNode = null;
            _goalNode = null;
            Debug.Log($"Exit Run Away\nRun Away {_entity.HasToRunAway}" +
                $"\nMove {_entity.HasToMoveInPath}\n" +
                $"Attack {_entity.HaveTargetToAttack()}");
            

        }

        public void OnLateUpdate()
        {
            _entity.FOV();
        }

        public void OnUpdate()
        {
            if (!_entity.HasToRunAway)
            {
                if (_entity.HaveTargetToAttack())
                    _fsm.ChangeState(FollowersEntitiesStates.Attack);
                else if (_entity.HasToMoveInPath)
                    _fsm.ChangeState(FollowersEntitiesStates.Seek);

                return;
            }

            _entity.FollowPath(_pathToFollow);
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
