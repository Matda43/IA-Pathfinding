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

    public void newSpawn()
    {
        if (spawnList.Count > 0)
        {
            int r = Random.Range(0, spawnList.Count);
            controller.transform.position = new Vector3(spawnList[r].x, spawnList[r].y + 1, spawnList[r].z);
            
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
