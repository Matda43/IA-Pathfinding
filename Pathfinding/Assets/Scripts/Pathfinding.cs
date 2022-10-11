using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Pathfinding
{
  
    int security = 4000;

    public List<Vector3> dijkstra(Dictionary<Vector3, Node> ground, Node startNode, Node targetNode)
    {
        if (startNode != null && targetNode != null)
        {
            List<Node> unexplored = new List<Node>();
            initDistance(ground, unexplored);

            startNode.setDistance(0);
            int i = 0;

            while (unexplored.Count > 0)
            {
                unexplored.Sort((x, y) => x.getDistance().CompareTo(y.getDistance()));
                Node current = unexplored[0];
                if (current == targetNode)
                {
                    return retracePath(startNode, targetNode);
                }

                unexplored.Remove(current);

                Dictionary<Node, float> neighbours = getNeighbours(ground, current.getPosition());
                foreach (var neighbour in neighbours)
                {
                    if (unexplored.Contains(neighbour.Key))
                    {
                        float distance = current.getDistance() + neighbour.Value;
                        if (distance < neighbour.Key.getDistance())
                        {
                            neighbour.Key.setDistance(distance);
                            neighbour.Key.setParent(current);
                        }
                    }
                }

                if (i > security)
                {
                    Debug.Log("Security mode");
                    return null;
                }
                i++;
            }
        }
        return null;
    }

    List<Vector3> retracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != null && currentNode != startNode)
        {
            path.Add(currentNode.getPosition());
            currentNode = currentNode.getParent();
        }
        path.Reverse();

        return path;
    }

    void initDistance(Dictionary<Vector3, Node> ground,  List<Node> list)
    {
        foreach (var item in ground)
        {
            item.Value.setDistance(1000);
            list.Add(item.Value);
        }
    }

    public Dictionary<Node, float> getNeighbours(Dictionary<Vector3, Node> ground, Vector3 position)
    {

        Vector3 x = new Vector3(1, 0, 0);
        Vector3 z = new Vector3(0, 0, 1);

        Dictionary<Vector3, float> adjacents = new Dictionary<Vector3, float>() {
            { position - x - z, Mathf.Sqrt(2) },
            { position - x + z, Mathf.Sqrt(2) },
            { position + x - z, Mathf.Sqrt(2) },
            { position + x + z, Mathf.Sqrt(2) },
            { position - x, 1 },
            { position + x, 1 },
            { position - z, 1 },
            { position + z, 1 }
        };

        if(!ground.ContainsKey(position - x))
        {
            adjacents.Remove(position - x + z);
            adjacents.Remove(position - x - z);
        }
        if (!ground.ContainsKey(position + x))
        {
            adjacents.Remove(position + x + z);
            adjacents.Remove(position + x - z);
        }
        if (!ground.ContainsKey(position - z))
        {
            adjacents.Remove(position - x - z);
            adjacents.Remove(position + x - z);
        }
        if (!ground.ContainsKey(position + x))
        {
            adjacents.Remove(position - x + z);
            adjacents.Remove(position + x + z);
        }

        Dictionary<Node, float> neighbours = new Dictionary<Node, float>();
        foreach (var adjacent in adjacents)
        {
            if (ground.ContainsKey(adjacent.Key))
            {
                neighbours.Add(ground[adjacent.Key], adjacent.Value);
            }
        }
        return neighbours;
    }


    float getDistance(Node n1, Node n2)
    {
        return Mathf.Max(Mathf.Abs(n2.getPosition().x - n1.getPosition().x), Mathf.Abs(n2.getPosition().z - n1.getPosition().z));
    }


    public List<Vector3> astar(Dictionary<Vector3, Node> ground, Node startNode, Node targetNode)
    {
        if (startNode != null && targetNode != null)
        {
            int i = 0;

            List<Node> openList = new List<Node>();
            List<Node> closeList = new List<Node>();
            startNode.setDistance(0);
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                openList.Sort((x, y) => x.getHeuristic().CompareTo(y.getHeuristic()));
                Node current = openList[0];
                if (current == targetNode)
                {
                    return retracePath(startNode, targetNode);
                }
                openList.Remove(current);
                closeList.Add(current);
                Dictionary<Node, float> neighbours = getNeighbours(ground, current.getPosition());
                foreach (var neighbour in neighbours)
                {
                    if (!openList.Contains(neighbour.Key) && !closeList.Contains(neighbour.Key))
                    {
                        neighbour.Key.setDistance(current.getDistance() + neighbour.Value);
                        neighbour.Key.setHeuristic(neighbour.Key.getDistance() + getDistance(neighbour.Key, targetNode));
                        neighbour.Key.setParent(current);
                        openList.Add(neighbour.Key);
                    }
                }

                if (i > security)
                {
                    Debug.Log("Security mode");
                    return null;
                }
                i++;
            }
        }
        return null;
    }
}