using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    public Material ground;
    public Material obstacles;
    public Material path;
    public bool g;
    public bool o;
    public int width = 20;
    public int height = 30;
    public Piece piece;

    Dictionary<Vector3, Node> dictGround = new Dictionary<Vector3, Node>();
    Dictionary<Vector3, Obstacle> dictObstacles = new Dictionary<Vector3, Obstacle>();
    Dictionary<Enemy, List<Vector3>> paths = new Dictionary<Enemy, List<Vector3>>();

    List<Vector3> spawnList;

    void Awake()
    {
        this.dictGround = generateDictGround();
        this.dictObstacles = generateDictObstacles(this.dictGround);
        generateMap(g, o);
        this.spawnList = generateSpawnList();
        this.piece.setPossibleSpawn(this.spawnList);
        this.piece.newSpawn();
    }

    public int getWidth()
    {
        return this.width;
    }

    public int getHeight()
    {
        return this.height;
    }


    List<Vector3> generateSpawnList()
    {
        List<Vector3> possibleSpawn = new List<Vector3>();
        foreach(var pair in dictGround)
        {
            if (pair.Value.isWalkable())
            {
                possibleSpawn.Add(pair.Key);
            }
        }
        return possibleSpawn;
    }

    public List<Vector3> getSpawnList()
    {
        return this.spawnList;
    }


    public void UpdatePath()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject g = GameObject.Find("Cube" + x + 0 + y);
                if (g != null)
                {
                    if (dictGround.ContainsKey(position))
                    {
                        Renderer r = g.GetComponent<Renderer>();
                        if (r.material != ground)
                        {
                            r.material = ground;
                        }
                        foreach (var item in paths)
                        {
                            if (item.Value != null && item.Value.Contains(position))
                            {
                                if (dictGround[position].isWalkable())
                                {
                                    if (g != null)
                                    {
                                        if (r.material != path)
                                        {
                                            r.material = path;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    Dictionary<Vector3, Node> generateDictGround()
    {
        Dictionary<Vector3, Node> dict = new Dictionary<Vector3, Node>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Node node = new Node(position, true);
                dict.Add(position, node);
            }
        }
        return dict;
    }

    void createAnObstacle(Dictionary<Vector3, Obstacle> dict, Dictionary<Vector3, Node> dictG, int x, int y)
    {
        Vector3 position = new Vector3(x, 1, y);
        if (!dict.ContainsKey(position))
        {
            Obstacle obstacle = new Obstacle(position, 2);
            dict.Add(position, obstacle);
            position.y = 0;
            if (dictG.ContainsKey(position))
            {
                dictG[position].setWalkable(false);
            }
        }
    }

    Dictionary<Vector3, Obstacle> generateDictObstacles(Dictionary<Vector3, Node> dictG)
    {
        Dictionary<Vector3, Obstacle> dict = new Dictionary<Vector3, Obstacle>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    createAnObstacle(dict, dictG, x, y);
                }
                int xo = Mathf.RoundToInt(Random.Range(1, width - 2));
                int yo = Mathf.RoundToInt(Random.Range(1, height - 2));
                if (xo % Mathf.RoundToInt(Random.Range(3, 5)) == 0 && yo % Mathf.RoundToInt(Random.Range(3, 4)) == 0)
                {
                    createAnObstacle(dict, dictG, xo, yo);
                }
            }
        }
        return dict;
    }

    public bool isWalkable(Vector3 position)
    {
        return dictGround.ContainsKey(position) && dictGround[position].isWalkable();
    }

    void displayGround(Vector3 position)
    {
        if (dictGround.ContainsKey(position))
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.GetComponent<Renderer>().sharedMaterial = ground;
            g.transform.localScale = new Vector3(1, .2f, 1);
            g.transform.position = new Vector3(position.x, position.y - .1f, position.z);

            g.name = "Cube" + position.x + position.y + position.z;
        }
    }

    void displayObstacles(Vector3 position)
    {
        if (dictObstacles.ContainsKey(position))
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.GetComponent<Renderer>().sharedMaterial = obstacles;
            o.transform.localScale = new Vector3(1, dictObstacles[position].getHeight(), 1);
            o.transform.position = position;
        }
    }

    public void generateMap(bool g, bool o)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (g)
                {
                    Vector3 position = new Vector3(x, 0, y);
                    displayGround(position);
                }
                if (o)
                {
                    Vector3 position = new Vector3(x, 1, y);
                    displayObstacles(position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                if (dictGround.ContainsKey(position))
                {
                    Gizmos.color = (dictGround[position].isWalkable()) ? Color.white : Color.red;

                    foreach (var item in paths)
                    {
                        if (item.Value != null && item.Value.Contains(position))
                        {
                            if (dictGround[position].isWalkable())
                            {
                                Gizmos.color = Color.black;
                            }
                        }
                    }

                    Gizmos.DrawCube(dictGround[position].getPosition(), new Vector3(1, 1, 1) * (1 - 0.1f));
                }
            }
        }
    }

    public void setPath(Enemy enemy, List<Vector3> path)
    {
        if (!paths.ContainsKey(enemy))
        {
            this.paths.Add(enemy, path);
        }
        else
        {
            this.paths[enemy] = path;
        }
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);
        Vector3 position = new Vector3(x, 0, y);
        if (dictGround.ContainsKey(position))
        {
            return dictGround[position];
        }
        else
        {
            return null;
        }
    }



    public void InitDistance(List<Node> list)
    {
        foreach (var item in dictGround)
        {
            if (item.Value.isWalkable())
            {
                item.Value.setDistance(1000);
                list.Add(item.Value);
            }
        }
    }

    public Dictionary<Node, float> GetNeighboursDijkstra(Vector3 position)
    {
        Dictionary<Vector3, float> adjacents = new Dictionary<Vector3, float>() {
            { new Vector3(position.x - 1, position.y, position.z - 1), Mathf.Sqrt(2) },
            { new Vector3(position.x - 1, position.y, position.z + 1), Mathf.Sqrt(2) },
            { new Vector3(position.x + 1, position.y, position.z - 1), Mathf.Sqrt(2) },
            { new Vector3(position.x + 1, position.y, position.z + 1), Mathf.Sqrt(2) },

            { new Vector3(position.x - 1, position.y, position.z), 1 },
            { new Vector3(position.x + 1, position.y, position.z), 1 },
            {  new Vector3(position.x, position.y, position.z - 1), 1 },
            { new Vector3(position.x, position.y, position.z + 1), 1 }
        };

        Dictionary<Node, float> neighbours = new Dictionary<Node, float>();

        foreach (var adjacent in adjacents)
        {
            if (dictGround.ContainsKey(adjacent.Key) && dictGround[adjacent.Key].isWalkable())
            {
                neighbours.Add(dictGround[adjacent.Key], adjacent.Value);
            }
        }
        return neighbours;
    }
}
