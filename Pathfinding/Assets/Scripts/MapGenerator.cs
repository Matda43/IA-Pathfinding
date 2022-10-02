using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator
{
    private Material materialGround;
    private Material materialObstacle;

    public MapGenerator(Material materialGround, Material materialObstacle)
    {
        this.materialGround = materialGround;
        this.materialObstacle = materialObstacle;
    }

    public Dictionary<Vector3, Node> generateGround(int width, int height)
    {
        Dictionary<Vector3, Node> dict = new Dictionary<Vector3, Node>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = new Node(new Vector3(x, 0, y));
                dict.Add(node.getPosition(), node);
            }
        }
        return dict;
    }

    void createAnObstacle(Dictionary<Vector3, Obstacle> obstacles, Dictionary<Vector3, Node> ground, int x, int y)
    {
        Vector3 position = new Vector3(x, 1, y);
        int height = 2;
        if (!obstacles.ContainsKey(position))
        {
            obstacles.Add(position, new Obstacle(position, height));
            position.y = 0;
            if (ground.ContainsKey(position))
            {
                ground.Remove(position);
            }
        }
    }

    public Dictionary<Vector3, Obstacle> generateObstacles(int width, int height, Dictionary<Vector3, Node> ground)
    {
        Dictionary<Vector3, Obstacle> obstacles = new Dictionary<Vector3, Obstacle>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    createAnObstacle(obstacles, ground, x, y);
                }
                int xo = Mathf.RoundToInt(Random.Range(1, width - 2));
                int yo = Mathf.RoundToInt(Random.Range(1, height - 2));
                if (xo % Mathf.RoundToInt(Random.Range(3, 5)) == 0 && yo % Mathf.RoundToInt(Random.Range(3, 4)) == 0)
                {
                    createAnObstacle(obstacles, ground, xo, yo);
                }
            }
        }
        return obstacles;
    }

    void displayGround(Dictionary<Vector3,Node> ground, Vector3 position)
    {
        if (ground.ContainsKey(position))
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.GetComponent<Renderer>().sharedMaterial = materialGround;
            g.transform.localScale = new Vector3(1, .2f, 1);
            g.transform.position = new Vector3(position.x, position.y - .1f, position.z);
            g.name = "Cube" + position.x + position.y + position.z;
        }
    }

    void displayObstacles(Dictionary<Vector3, Obstacle> obstacles, Vector3 position)
    {
        if (obstacles.ContainsKey(position))
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.GetComponent<Renderer>().sharedMaterial = materialObstacle;
            o.transform.localScale = new Vector3(1, obstacles[position].getHeight(), 1);
            o.transform.position = position;
        }
    }

    public void generateMap(int width, int height, Dictionary<Vector3, Node> ground, Dictionary<Vector3, Obstacle> obstacles, bool g, bool o)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (g)
                {
                    displayGround(ground, new Vector3(x, 0, y));
                }
                if (o)
                {
                    displayObstacles(obstacles, new Vector3(x, 1, y));
                }
            }
        }
    }

}
