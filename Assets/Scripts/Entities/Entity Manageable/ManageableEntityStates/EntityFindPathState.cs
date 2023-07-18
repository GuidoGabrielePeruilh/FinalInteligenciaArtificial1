using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFindPathState : IState
{
    FSM<ManageableEntityStates> _fsm;

    Node _startingNode, _goalNode;

    Stack<Node> _pathToFollow;
    PathFinding _pathfinding;

    ManageableEntities _entity;

    public EntityFindPathState(FSM<ManageableEntityStates> fsm, ManageableEntities entity)
    {
        _fsm = fsm;
        _entity = entity;
    }
    public void OnEnter()
    {
        Debug.Log("FindPath");
        Debug.Log(_entity.transform.position);
        _startingNode = NodesManager.Instance.SetNode(_entity.transform.position);
        _goalNode = NodesManager.Instance.SetNode(_entity.TargetPosition);
        _pathToFollow = new Stack<Node>();
        _pathfinding = new PathFinding();
        _pathToFollow = _pathfinding.AStar(_startingNode, _goalNode);
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
    }
}
