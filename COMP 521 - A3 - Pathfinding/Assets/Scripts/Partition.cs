using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Partition
{
    Vector2 position;
    GameObject occupant;
    List<Edge> edges;

    public Partition(Vector2 pos)
    {
        position = pos;
        occupant = null;
        edges = new List<Edge>();
    }

    public Vector2 GetPosition()
    {
        return position;
    }
}
