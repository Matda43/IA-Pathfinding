using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{

    public Player player;
    public Enemy[] enemies;
    Map map;
    int security = 4000;
    int count = 10;

    Node nodeRemember;

    private void Awake()
    {
        map = GetComponent<Map>();
    }

    private void Update()
    {
        Node node = map.NodeFromWorldPoint(player.transform.localPosition);
        if (node != nodeRemember)
        {
            foreach (Enemy enemy in enemies)
            {
                FindPath(player, enemy);
            }
            map.UpdatePath();
            nodeRemember = node;
        }
        else
        {
            if (count < security)
            {
                count += 100;
            }
            else
            {
                foreach (Enemy enemy in enemies)
                {
                    FindPath(player, enemy);
                }
                map.UpdatePath();
                count = 0;
            }
        }
    }


    void FindPath(Player player, Enemy enemy)
    {
        Vector3 startPos = player.transform.localPosition;
        Vector3 targetPos = enemy.transform.localPosition;
        Node startNode = map.NodeFromWorldPoint(startPos);
        Node targetNode = map.NodeFromWorldPoint(targetPos);

        if (startNode != null && map.isWalkable(startNode.getPosition()) && targetNode != null && map.isWalkable(targetNode.getPosition()))
        {
            List<Node> unexplored = new List<Node>();
            map.InitDistance(unexplored);

            startNode.setDistance(0);
            int i = 0;

            while (unexplored.Count > 0)
            {
                unexplored.Sort((x, y) => x.getDistance().CompareTo(y.getDistance()));
                Node current = unexplored[0];
                if (current == targetNode)
                {
                    RetracePath(enemy, startNode, targetNode);
                    return;
                }

                unexplored.Remove(current);

                Dictionary<Node, float> neighbours = map.GetNeighboursDijkstra(current.getPosition());
                foreach (var neighbour in neighbours)
                {
                    if (unexplored.Contains(neighbour.Key) && neighbour.Key.isWalkable())
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
                    return;
                }
                i++;
            }
        }
    }

    void RetracePath(Enemy enemy, Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            currentNode = currentNode.getParent();
            path.Add(currentNode.getPosition());
        }

        map.setPath(enemy, path);
        if (path.Count > 0)
        {
            setDirectionEnemy(enemy, path[0]);
        }
    }

    void setDirectionEnemy(Enemy enemy, Vector3 nextPosition)
    {
        int x = (enemy.transform.localPosition.x > nextPosition.x + .1f) ? -1 : (enemy.transform.localPosition.x < nextPosition.x - .1f) ? 1 : 0;
        int z = (enemy.transform.localPosition.z > nextPosition.z + .1f) ? -1 : (enemy.transform.localPosition.z < nextPosition.z - .1f) ? 1 : 0;
        enemy.setDirection(new Vector3(x, 0, z));
    }
}