using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesManager : MonoBehaviour
{
    public static NodesManager Instance { get; private set; }
    [SerializeField] public LayerMask BlockedNodeLayer { get; private set; }
    Node[] _nodes;
    Node _endNode;

    private void Awake()
    {
        Instance = this;
        _nodes = FindObjectsOfType<Node>();
    }

    public Node SetTargetGoalNode(Vector3 positionOfMyTarget)
    {
        float _minDistance = 1000000f;

        for (int i = 0; i < _nodes.Length; i++)
        {
            float disToTarget = Vector3.Distance(_nodes[i].transform.position, positionOfMyTarget);

            if (disToTarget < _minDistance)
            {
                _minDistance = disToTarget;
                _endNode = _nodes[i];
            }
        }
        return _endNode;
    }
}
