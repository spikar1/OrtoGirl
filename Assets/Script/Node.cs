using UnityEngine;
using System.Collections;
using System;

public class Node : IHeapItem<Node> {

    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gridZ;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public GameObject currentObject = null;


    public bool[] walkable = new bool[6];

    public Node(Vector3 _worldPos, int _gridX, int _gridY, int _gridZ)
    {
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        gridZ = _gridZ;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

   

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
