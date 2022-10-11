using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node
{
    Vector3 position;
    float distance;
    Node parent;
    float heuristic;

    public Node(Vector3 position)
    {
        this.position = position;
        this.heuristic = 0;
    }

    public void setParent(Node parent)
    {
        this.parent = parent;
    }

    public void setHeuristic(float new_heuristic)
    {
        this.heuristic = new_heuristic;
    }

    public float getHeuristic()
    {
        return this.heuristic;
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

    public Vector3 getPosition()
    {
        return this.position;
    }
}
