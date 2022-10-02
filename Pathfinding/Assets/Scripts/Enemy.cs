using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyController))]

public class Enemy : MonoBehaviour
{
    public Material materialPath;
    public TextMeshPro pathfindingText;
    public bool dijkstra;
    public bool astar;

    Dictionary<Vector3, Node> spawnList;

    [Range(1f, 6f)]
    public float moveSpeed = 5;

    EnemyController controller;
    Vector3 direction;
    Pathfinding pathfinding;
    Vector3 positionPlayer;
    List<Vector3> path;
    int cptmax = 10;
    int cpt = 0;

    void Awake()
    {
        this.path = new List<Vector3>();
        this.pathfinding = new Pathfinding();
        this.spawnList = new Dictionary<Vector3, Node>();
        this.direction = new Vector3(0, 0, 0);
        //moveSpeed = Mathf.RoundToInt(Random.Range(3, 4));
        controller = GetComponent<EnemyController>();
        controller.transform.position = new Vector3(10, 0.5f, 7);
        if(dijkstra == astar){
            int r = Mathf.RoundToInt(Random.Range(0, 100));
            if(r%2 == 0)
            {
                dijkstra = true;
                astar = false;
            }
            else
            {
                astar = true;
                dijkstra = false;
            }
        }
        this.pathfindingText.text = dijkstra ? "D" : "A";
    }

    public void setPositionPlayer(Vector3 positionPlayer)
    {
        this.positionPlayer = positionPlayer;
    }

    public void setPossibleSpawn(Dictionary<Vector3, Node> spawnList)
    {
        this.spawnList = spawnList;
    }

    public void newSpawn()
    {
        if (spawnList.Count > 0)
        {
            int r = Random.Range(0, spawnList.Count);
            controller.transform.position = new List<Vector3>(this.spawnList.Keys)[r];
        }
    }

    void Update()
    {
        if (this.spawnList != null && this.positionPlayer != null && this.cpt >= cptmax)
        {
            List<Vector3> p;
            if (dijkstra)
            {
                p = pathfinding.dijkstra(this.spawnList, NodeFromWorldPoint(transform.localPosition), NodeFromWorldPoint(this.positionPlayer));
            }
            else
            {
                p = pathfinding.astar(this.spawnList, NodeFromWorldPoint(transform.localPosition), NodeFromWorldPoint(this.positionPlayer));
            }
            if (p != null && p.Count > 0)
            {
                this.path = p;
                calculDirection();
                Vector3 moveVelocity = direction.normalized * moveSpeed;
                controller.Move(moveVelocity);
            }
            cpt = 0;
        }
        else
        {
            this.cpt++;
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);
        Vector3 position = new Vector3(x, 0, y);
        if (spawnList.ContainsKey(position))
        {
            return spawnList[position];
        }
        else
        {
            return null;
        }
    }

    void calculDirection()
    {
        Vector3 nextPosition = path[0];
        int x = (transform.localPosition.x > nextPosition.x + .1f) ? -1 : (transform.localPosition.x < nextPosition.x - .1f) ? 1 : 0;
        int z = (transform.localPosition.z > nextPosition.z + .1f) ? -1 : (transform.localPosition.z < nextPosition.z - .1f) ? 1 : 0;
        setDirection(new Vector3(x, 0, z));
    }

    public void setDirection(Vector3 v)
    {
        direction = v;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent(typeof(Player)) as Player)
        {
            var enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.newSpawn();
            }
        }
    }

    public List<Vector3> getPath()
    {
        return this.path;
    }

    public Material getMaterial()
    {
        return materialPath;
    }
}
