using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool IsBlocked { get; private set; }
    public int cost = 1;
    private List<Node> _neighbors = new List<Node>();
    private NodesCreator _grid;
    private Vector2Int _gridPosition;
    [SerializeField] private LayerMask collisionLayer;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, collisionLayer);

        if (colliders.Length > 0)
        {
            SetBlocked(true);
        }
        else
        {
            SetBlocked(false);
        }
    }

    public void Initialize(NodesCreator grid, Vector2Int gridPosition)
    {
        _grid = grid;
        _gridPosition = gridPosition;
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
        gameObject.layer = isBlock ? 6 : 0;
    }

    
}
