using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node
{
    Vector3 position;
    bool walkable;
    float distance;
    Node parent;

    public Node(Vector3 position, bool walkable)
    {
        this.position = position;
        this.walkable = walkable;
    }

    public void setParent(Node parent)
    {
        this.parent = parent;
    }

    public Node getParent()
    {
        return parent;
    }

    public void setDistance(float new_distance)
    {
        this.distance = new_distance;
    }

    public void addDistance(float new_distance)
    {
        this.distance += new_distance;
    }

    public float getDistance()
    {
        return distance;
    }

    public void setWalkable(bool walkable)
    {
        this.walkable = walkable;
    }

    public bool isWalkable()
    {
        return this.walkable;
    }

    public Vector3 getPosition()
    {
        return this.position;
    }
}
