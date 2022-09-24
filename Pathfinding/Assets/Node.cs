using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public Node parent;
    public int dst;
    private int dstMax = 10000;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        this.dst = dstMax;
    }

    public void initDst()
    {
        this.dst = this.dstMax;
        this.parent = null;
    }
}