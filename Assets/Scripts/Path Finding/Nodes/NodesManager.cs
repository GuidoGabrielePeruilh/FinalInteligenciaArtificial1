using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodesManager : MonoBehaviour
{
    public static NodesManager Instance { get; private set; }
    public LayerMask BlockedNodeLayer => _blockedNodeLayer;
    [SerializeField] private LayerMask _blockedNodeLayer;
    //[SerializeField] private float _radiusToFindNode = 0.5f;
    [SerializeReference] private List<Node> _validNodes;
    [SerializeReference] private List<Node> _invalidNodes;
    private Node _node;

    private void Awake()
    {
        Instance = this;
    }

    public void SuscribeNode(Node node)
    {
        _validNodes.Add(node);
    }

    public void RemoveNode(Node node)
    {
        if (!_validNodes.Contains(node)) return;
        _validNodes.Remove(node);
    }

    public void SuscribeBlockedNode(Node node)
    {
        _invalidNodes.Add(node);
    }

    public void RemoveBlockedNode(Node node)
    {
        if (!_invalidNodes.Contains(node)) return;
        _invalidNodes.Remove(node);
    }

    public Node SetNode(Vector3 position)
    {
        float _minDistance = 1000000f;

        foreach (var node in _validNodes)
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

    public IEnumerable<Node> GetAllNodes()
    {
        return _validNodes.Concat(_invalidNodes);
    }
}
