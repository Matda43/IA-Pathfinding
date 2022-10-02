using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(PieceController))]

public class Piece : MonoBehaviour
{
    List<Vector3> spawnList;
    PieceController controller;
    int cptmax = 10;
    int cpt = 0;

    // Start is called before the first frame update
    void Awake()
    {
        spawnList = new List<Vector3>();
        controller = GetComponent<PieceController>();
    }

    public void setPossibleSpawn(List<Vector3> spawnList)
    {
        this.spawnList = spawnList;
    }

    private void Update()
    {
        if (cpt >= cptmax)
        {
            if (transform.position.y < -5)
            {
                newSpawn();
            }
            cpt = 0;
        }
        else
        {
            cpt++;
        }
    }

    public void newSpawn()
    {
        if (spawnList.Count > 0)
        {
            int r = Random.Range(0, spawnList.Count);
            Vector3 new_position = spawnList[r];
            new_position.y = 3;
            controller.transform.position = new_position;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent(typeof(Player)) as Player)
        {
            newSpawn();
        }
    }
}
