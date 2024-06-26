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

        public void OnLateUpdate()
        {
            _entity.FOV();
        }

        public void OnUpdate()
        {
            _entity.FollowPath(_pathToFollow);

            if (_entity.HasArriveToDestiny)
            {
                _fsm.ChangeState(ManageableEntityStates.Idle);
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

