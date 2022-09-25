using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle
{
    Vector3 position;
    int height;

    public Obstacle(Vector3 position, int height)
    {
        this.position = position;
        this.height = height;
    }

    public int getHeight()
    {
        return this.height;
    }
    public Vector3 getPosition()
    {
        return this.position;
    }
}
