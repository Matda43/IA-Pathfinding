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
    Vector3 spawn = new Vector3(2, 0.5f, 2);

    void Start()
    {
        controller = GetComponent<PlayerController>();
        controller.transform.position = spawn;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent(typeof(Enemy)) as Enemy)
        {
            controller.transform.position = spawn;

            score = 0;
            scoreText.text = score.ToString();
        }
        else if (collision.gameObject.GetComponent(typeof(Piece)) as Piece)
        {
            score = (score + 1) % 10;
            scoreText.text = score.ToString();
        }
    }
}
