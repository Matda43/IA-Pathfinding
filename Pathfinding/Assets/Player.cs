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
    float moveSpeed = 5;

    PlayerController controller;
    new Camera camera;
    //Vector3 spawn = new Vector3(2, 0.5f, 2);
    List<Vector3> spawnList;

    void Start()
    {
        this.spawnList = new List<Vector3>();
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

    public void setPossibleSpawn(List<Vector3> spawnList)
    {
        this.spawnList = spawnList;
    }

    public void newSpawn()
    {
        if (spawnList.Count > 0)
        {
            int r = Random.Range(0, spawnList.Count);
            controller.transform.position = new Vector3(spawnList[r].x, spawnList[r].y + 0.5f, spawnList[r].z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent(typeof(Enemy)) as Enemy)
        {
            score = 0;
            newSpawn();
            scoreText.text = score.ToString();
        }
        else if (collision.gameObject.GetComponent(typeof(Piece)) as Piece)
        {
            score = (score + 1) % 10;
            scoreText.text = score.ToString();
        }
    }
}
