using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    public Material materialGround;
    public Material materialObstacle;
    public bool g = true;
    public bool o = true;

    [Range(3, 30)]
    public int width = 20;
    
    [Range(3, 30)]
    public int height = 30;

    public Player player;
    public Enemy[] enemies;
    public Piece piece;

    MapGenerator generator;
    Dictionary<Vector3, Node> ground = new Dictionary<Vector3, Node>();
    Dictionary<Vector3, Obstacle> obstacles = new Dictionary<Vector3, Obstacle>();
    Dictionary<Enemy, List<Vector3>> paths = new Dictionary<Enemy, List<Vector3>>();

    int cptmax = 10;
    int cpt = 0;

    void Start()
    {
        generator = new MapGenerator(materialGround, materialObstacle);
        this.ground = generator.generateGround(this.width, this.height);
        this.obstacles = generator.generateObstacles(this.width, this.height, this.ground);
        generator.generateMap(this.width, this.height, this.ground, this.obstacles, g, o);
        initPlayer();
        initEnemies();
        this.piece.setPossibleSpawn(new List<Vector3>(this.ground.Keys));
        this.piece.newSpawn();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            if (cpt >= cptmax)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.setPositionPlayer(getPositionPlayer());
                }
                UpdatePath(enemies);
                cpt = 0;
            }
            else
            {
                cpt++;
            }
        }
    }

    void initPlayer()
    {
        player.setPossibleSpawn(ground);
        player.newSpawn();
    }

    void initEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            Dictionary<Vector3, Node> nodes = new Dictionary<Vector3, Node>(ground);
            enemy.setPossibleSpawn(nodes);
            enemy.newSpawn();
            enemy.setPositionPlayer(getPositionPlayer());
        }
    }

    public Vector3 getPositionPlayer()
    {
        return player.transform.localPosition;
    }


    void UpdatePath(Enemy[] enemies)
    {
        foreach (var item in ground)
        {
            GameObject g = GameObject.Find("Cube" + item.Key.x + 0 + item.Key.z);
            if (g != null)
            {
                Renderer r = g.GetComponent<Renderer>();
                if (r.material != materialGround)
                {
                    r.material = materialGround;
                }
                foreach (Enemy e in enemies)
                {
                    if (e.getPath().Contains(item.Key))
                    {
                        r.material = e.getMaterial();
                        break;
                    }
                }
            }
        }
    }

    /*
    private void OnDrawGizmos()
    {
        foreach (var item in ground) {
            
            Gizmos.color = Color.white;

            foreach (Enemy e in enemies)
            {
                if (e.getPath().Contains(item.Key))
                {
                    Gizmos.color = Color.black;
                    break;
                }
            }
            Gizmos.DrawCube(item.Key, new Vector3(1, 1, 1) * (1 - 0.1f));
        }
    }
    */
}
