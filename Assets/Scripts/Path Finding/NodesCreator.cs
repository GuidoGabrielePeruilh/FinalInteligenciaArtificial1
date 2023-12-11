using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodesCreator : MonoBehaviour
{
    [SerializeReference] Node[,] _grid;

    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] GameObject nodePrefab;

    [Range(0f, 5f), SerializeField]
    float _nodeOffset;

    private void Awake()
    {
        DeleteAllNodes();
        CreateGrid();
    }

    [ContextMenu("Nodes Creator/Create Grid")]
    void CreateGrid()
    {
        _grid = new Node[_width, _height];

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GameObject obj = Instantiate(nodePrefab,this.transform);

                obj.name = "Node_" + i + "_" + j;

                obj.transform.position = new Vector3(i * _nodeOffset, -1, j * _nodeOffset);

                Node node = obj.GetComponent<Node>();

                node.Initialize(this, new Vector2Int(i, j), _collisionLayer);

                _grid[i, j] = node;
            }
        }
    }

    [ContextMenu("Nodes Creator/Delete Grid")]
    private void DeleteAllNodes()
    {
        var nodesToDelete = FindObjectsOfType<Node>();

        foreach (var node in nodesToDelete)
        {
            DestroyImmediate(node.gameObject);
        }
    }

    public List<Node> GetNeighborsFromPosition(int row, int column)
    {
        List<Node> neighbors = new List<Node>();
        if (column + 1 < _height) neighbors.Add(_grid[row, column + 1]);
        if (row + 1 < _width) neighbors.Add(_grid[row + 1, column]);
        if (column - 1 >= 0) neighbors.Add(_grid[row, column - 1]);
        if (row - 1 >= 0) neighbors.Add(_grid[row - 1, column]);

        return neighbors;
    }
}
