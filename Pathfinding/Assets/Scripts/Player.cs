using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public TextMeshPro scoreText;

    int score;

    [Range(1f, 6f)]
    public float moveSpeed = 5;

    PlayerController controller;
    new Camera camera;
    Dictionary<Vector3, Node> spawnList;

    void Start()
    {
        this.spawnList = new Dictionary<Vector3, Node>();
        controller = GetComponent<PlayerController>();
        //controller.transform.position = spawn;

        camera = Camera.main;
        camera.transform.Rotate(new Vector3(90, 0, 0));

        score = 0;
        scoreText.text = score.ToString();
    }

    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);
        camera.transform.position = new Vector3(controller.getPosition().x, 10, controller.getPosition().z);

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
            controller.transform.position = new List<Vector3>(spawnList.Keys)[r]; //new Vector3(1,0.5f,1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent(typeof(Enemy)) as Enemy)
        {
            score = 0;
            scoreText.fontSize = 10;
            newSpawn();
            scoreText.text = score.ToString();
        }
        else if (collision.gameObject.GetComponent(typeof(Piece)) as Piece)
        {
            score++;
            if(score == 10 || score == 1000)
            {
                scoreText.fontSize = scoreText.fontSize / 2;
            }
            scoreText.text = score.ToString();
        }
    }
}
