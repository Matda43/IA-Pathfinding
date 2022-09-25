using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyController))]

public class Enemy : MonoBehaviour
{
    float moveSpeed;

    EnemyController controller;
    Vector3 direction = new Vector3(0, 0, 0);
    Vector3 spawn;

    void Start()
    {
        spawn = newSpawn();
        moveSpeed = Mathf.RoundToInt(Random.Range(3, 5));
        controller = GetComponent<EnemyController>();
        controller.transform.position = spawn;
    }

    Vector3 newSpawn()
    {
        return new Vector3(Mathf.RoundToInt(Random.Range(5, 18)), 0.5f, Mathf.RoundToInt(Random.Range(1, 8)));
    }

    void Update()
    {
        Vector3 moveInput = direction;
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);
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
                enemy.transform.position = newSpawn();
            }
        }
    }

}
