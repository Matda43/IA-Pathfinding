using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyController))]

public class Enemy : MonoBehaviour
{
    List<Vector3> spawnList;
    float moveSpeed;

    EnemyController controller;
    Vector3 direction;

    void Start()
    {
        this.spawnList = new List<Vector3>();
        this.direction = new Vector3(0, 0, 0);
        moveSpeed = Mathf.RoundToInt(Random.Range(4, 5));
        controller = GetComponent<EnemyController>();
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
                enemy.newSpawn();
            }
        }
    }

}
