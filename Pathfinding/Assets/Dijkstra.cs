using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
    int security = 4000;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }


    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> unexplored = new List<Node>();
        grid.InitDst(unexplored);

        startNode.dst = 0;

        int i = 0;
        while (unexplored.Count > 0)
        {
            unexplored.Sort((x, y) => x.dst.CompareTo(y.dst));
            Node current = unexplored[0];
            if (current == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            unexplored.Remove(current);

            List<Node> neighbours = grid.GetNeighboursDijkstra(current);
            foreach (Node neighbour in neighbours)
            {
                if (unexplored.Contains(neighbour) && neighbour.walkable)
                {
                    int distance = current.dst + 1;
                    if (distance < neighbour.dst)
                    {
                        neighbour.dst = distance;
                        neighbour.parent = current;
                    }
                }
            }

            if (i > security)
            {
                Debug.Log("Security mode");
                return;
            }
            i++;
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;


        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }
}