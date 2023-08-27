using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool IsBlocked { get; private set; }
    public int cost = 1;
    private List<Node> _neighbors = new List<Node>();
    [SerializeReference] private NodesCreator _grid;
    [SerializeReference] private Vector2Int _gridPosition;
    [SerializeReference] private LayerMask _collisionLayer;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, _collisionLayer);

        if (colliders.Length > 0)
        {
            SetBlocked(true);
        }
        else
        {
            SetBlocked(false);
            NodesManager.Instance.SuscribeNode(this);
        }
    }

    public void Initialize(NodesCreator grid, Vector2Int gridPosition, LayerMask collisionLayer)
    {
        _grid = grid;
        _gridPosition = gridPosition;
        _collisionLayer = collisionLayer;
    }

    public List<Node> GetNeighbors()
    {
        if (_neighbors.Count == 0)
            _neighbors = _grid.GetNeighborsFromPosition(_gridPosition.x, _gridPosition.y);

        return _neighbors;
    }

    void SetBlocked(bool isBlock)
    {
        IsBlocked = isBlock;
        gameObject.layer = isBlock ? 6 : 7;
    }

    
}
