using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    float Heuristic(Node currentNode, Node goalNode)
    {
        return Mathf.Abs(currentNode.transform.position.x - goalNode.transform.position.x) +
                    Mathf.Abs(currentNode.transform.position.z - goalNode.transform.position.z);
    }

    public List<Node> ThetaStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        List<Node> path = new List<Node>(AStar(startingNode, goalNode));

        int current = 0;

        while (current + 2 < path.Count)
        {
            if (InLineOfSight(start: path[current].transform.position, end: path[current + 2].transform.position))
            {
                path.RemoveAt(current + 1);
            }
            else
            {
                current++;
            }
        }

        return path;
    }

    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, NodesManager.Instance.BlockedNodeLayer);
    }

    public Stack<Node> AStar(Node startingNode, Node goalNode)
    {
        Stack<Node> path = new Stack<Node>();

        if (startingNode == null || goalNode == null) return path;
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                while (current != null)
                {
                    path.Push(current);
                    current = cameFrom[current];
                }

                return path;
            }

            foreach (var next in current.GetNeighbors())
            {
                int newCost = costSoFar[current] + next.cost;

                float priority = newCost + Heuristic(next, goalNode);

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }
        return path;
    }
}
