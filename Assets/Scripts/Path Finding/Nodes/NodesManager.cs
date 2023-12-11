using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA_I;

public class NodesManager : Singleton<NodesManager>
{
    public enum NodesLists
    {
        Used,
        Valid,
        Invalid,
    }
    public List<Node> UsedNodes { get; private set; }
    public LayerMask BlockedNodeLayer => _blockedNodeLayer;
    [SerializeField] private LayerMask _blockedNodeLayer;
    [SerializeReference] private List<Node> _validNodes;
    [SerializeReference] private List<Node> _invalidNodes;
    private Node _node;

    new private void Awake()
    {
        itDestroyOnLoad = true;
        base.Awake();
        UsedNodes = new List<Node>();
    }

    public void RegistrerNewUsedNode(Node node)
    {
        if (UsedNodes.Contains(node)) return;
        UsedNodes.Add(node);
    }

    public void RemoveUsedNode(Node node)
    {
        if (!UsedNodes.Contains(node)) return;
        UsedNodes.Remove(node);
    }

    public void SuscribeNode(Node node)
    {
        if (_validNodes.Contains(node)) return;
        _validNodes.Add(node);
    }

    public void RemoveNode(Node node)
    {
        if (!_validNodes.Contains(node)) return;
        _validNodes.Remove(node);
    }

    public void SuscribeBlockedNode(Node node)
    {
        if (_invalidNodes.Contains(node)) return;
        _invalidNodes.Add(node);
    }

    public void RemoveBlockedNode(Node node)
    {
        if (!_invalidNodes.Contains(node)) return;
        _invalidNodes.Remove(node);
    }

    public bool CheckExistingNode(NodesLists nodeListType, Node node)
    {
        return nodeListType switch
        {
            NodesLists.Used => UsedNodes.Contains(node),
            NodesLists.Valid => _validNodes.Contains(node),
            NodesLists.Invalid => _invalidNodes.Contains(node),
            _ => false,
        };
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
