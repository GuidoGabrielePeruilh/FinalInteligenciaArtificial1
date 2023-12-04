using IA_I.EntityNS.Manegeable;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.FSM.StatesBehaviour
{
    public class EntityMoveState : IState
    {
        FSM<ManageableEntityStates> _fsm;
        Node _startingNode, _goalNode;
        Stack<Node> _pathToFollow;
        PathFinding _pathfinding;
        Vector3 _targetPosition;
        ManageableEntities _entity;

        public EntityMoveState(FSM<ManageableEntityStates> fsm, ManageableEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }
        public void OnEnter()
        {
            _targetPosition = _entity.TargetPosition;
            UpdatePath();
        }

        public void OnExit()
        {
            _startingNode = null;
            _goalNode = null;
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

            if (!_entity.HasToMove)
            {
                _fsm.ChangeState(ManageableEntityStates.Idle);
                return;
            }

            _entity.FollowPath(_pathToFollow);
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
            _pathToFollow = _pathfinding.AStar(_startingNode, _goalNode);
        }

    }
}

