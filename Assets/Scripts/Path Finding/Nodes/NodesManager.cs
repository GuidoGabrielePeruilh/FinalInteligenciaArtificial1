using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesManager : MonoBehaviour
{
    public static NodesManager Instance { get; private set; }
    [SerializeField] public LayerMask BlockedNodeLayer { get; private set; }
    Node[] _nodes;
    Node _node;
    [SerializeField] private LayerMask collisionLayer;

    private void Awake()
    {
        Instance = this;
    }

    public Node SetNode(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f, collisionLayer);
        Debug.Log(colliders.Length);
        float _minDistance = 1000000f;

        for (int i = 0; i < colliders.Length; i++)
        {
            float disToTarget = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (disToTarget < _minDistance)
            {
                _minDistance = disToTarget;
                _node = colliders[i].GetComponent<Node>();
            }
        }
        return _node;
    }
}
