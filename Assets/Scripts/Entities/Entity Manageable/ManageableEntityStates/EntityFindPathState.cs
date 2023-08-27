using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFindPathState : IState
{
    FSM<ManageableEntityStates> _fsm;
    Node _startingNode, _goalNode;
    Stack<Node> _pathToFollow;
    PathFinding _pathfinding;
    Vector3 _targetPosition;
    ManageableEntities _entity;

    public EntityFindPathState(FSM<ManageableEntityStates> fsm, ManageableEntities entity)
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
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
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
        Debug.Log($"UpdatePath startingNode {_startingNode.name}, goalNode {_goalNode.name}");
    }

}
