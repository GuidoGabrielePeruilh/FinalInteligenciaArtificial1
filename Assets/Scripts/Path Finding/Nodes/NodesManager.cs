using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesManager : MonoBehaviour
{
    public static NodesManager Instance { get; private set; }
    [SerializeField] public LayerMask BlockedNodeLayer { get; private set; }
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float _radiusToFindNode = 0.5f;
    [SerializeReference] private List<Node>_nodes;
    private Node _node;

    private void Awake()
    {
        Instance = this;
    }

    public void SuscribeNode(Node node)
    {
        _nodes.Add(node);
    }

    public Node SetNode(Vector3 position)
    {
        float _minDistance = 1000000f;

        foreach (var node in _nodes)
        {
            float disToTarget = Vector3.Distance(node.transform.position, position);
            if (disToTarget < _minDistance)
            {
                _minDistance = disToTarget;
                _node = node;
            }
        }

        return _node;
    }

}
