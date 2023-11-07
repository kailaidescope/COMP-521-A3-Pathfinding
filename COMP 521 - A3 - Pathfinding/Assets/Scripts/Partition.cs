using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Partition
{
    Vector3 position;
    GameObject occupied;
    List<Edge> edges;

    public Partition(Vector3 pos)
    {
        position = pos;
        occupied = null;
        edges = new List<Edge>();
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public List<Edge> GetEdges()
    {
        return edges;
    }

    public GameObject GetOccupied() { return occupied; }
    public Partition SetOccupied(GameObject g) { occupied = g; return this; }

    public List<Partition> GetConnectedPartitions()
    {
        List<Partition> connectedParts = new List<Partition>();

        foreach(Edge e in edges)
        {
            connectedParts.Add((e.a == this)? e.b : e.a);
        }

        return connectedParts;
    }

    public float GetDistanceToPartition(Partition part)
    {
        foreach(Edge e in edges)
        {
            if (e.a == part || e.b == part)
            {
                return e.distance;
            }
        }

        throw new Exception("Partition not connected");
    }

    public void DrawWithEdges(Color partColor, Color edgeColor)
    {
        float[] xs = {position.x-0.5f, position.x+0.5f};
        float[] zs = {position.z-0.5f, position.z+0.5f};
        
        Debug.DrawLine(new Vector3(xs[0], 0, zs[0]), new Vector3(xs[0], 0, zs[1]), partColor, 100f);
        Debug.DrawLine(new Vector3(xs[0], 0, zs[1]), new Vector3(xs[1], 0, zs[1]), partColor, 100f);
        Debug.DrawLine(new Vector3(xs[1], 0, zs[0]), new Vector3(xs[0], 0, zs[0]), partColor, 100f);
        Debug.DrawLine(new Vector3(xs[1], 0, zs[1]), new Vector3(xs[1], 0, zs[0]), partColor, 100f);

        foreach (Edge e in edges)
        {
            if(e.distance > 0)
            {
                if (e.a == this)
                {
                    if (e.b.position.x < position.x || e.b.position.y < position.y)
                    {
                        Debug.DrawLine(e.b.position, e.a.position, edgeColor, 100f);
                    }
                } else 
                {
                    if (e.a.position.x < position.x || e.a.position.y < position.y)
                    {
                        Debug.DrawLine(e.b.position, e.a.position, edgeColor, 100f);
                    }
                }
            }
        }
    }
    
    public void Draw(Color partColor)
    {
        float[] xs = {position.x-0.5f, position.x+0.5f};
        float[] zs = {position.z-0.5f, position.z+0.5f};
        
        Debug.DrawLine(new Vector3(xs[0], 0, zs[0]), new Vector3(xs[0], 0, zs[1]), partColor);
        Debug.DrawLine(new Vector3(xs[0], 0, zs[1]), new Vector3(xs[1], 0, zs[1]), partColor);
        Debug.DrawLine(new Vector3(xs[1], 0, zs[0]), new Vector3(xs[0], 0, zs[0]), partColor);
        Debug.DrawLine(new Vector3(xs[1], 0, zs[1]), new Vector3(xs[1], 0, zs[0]), partColor);
    }

    public override string ToString()
    {
        return "Partition at "+position.ToString();
    }
}
