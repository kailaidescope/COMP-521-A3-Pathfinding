using System;
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

    public List<Edge> GetEdges()
    {
        return edges;
    }

    public void Draw(Color partColor, Color edgeColor)
    {
        float[] xs = {position.x-0.5f, position.x+0.5f};
        float[] ys = {position.y-0.5f, position.y+0.5f};
        
        Debug.DrawLine(new Vector3(xs[0], 0, ys[0]), new Vector3(xs[0], 0, ys[1]), partColor, 100f);
        Debug.DrawLine(new Vector3(xs[0], 0, ys[1]), new Vector3(xs[1], 0, ys[1]), partColor, 100f);
        Debug.DrawLine(new Vector3(xs[1], 0, ys[0]), new Vector3(xs[0], 0, ys[0]), partColor, 100f);
        Debug.DrawLine(new Vector3(xs[1], 0, ys[1]), new Vector3(xs[1], 0, ys[0]), partColor, 100f);

        foreach (Edge e in edges)
        {
            if(e.distance > 1)
            {
                if (e.a == this)
                {
                    if (e.b.position.x < position.x || e.b.position.y < position.y)
                    {
                        Debug.DrawLine(new Vector3(e.b.position.x, 0, e.b.position.y), new Vector3(e.a.position.x, 0, e.a.position.y), edgeColor, 100f);
                    }
                } else 
                {
                    if (e.a.position.x < position.x || e.a.position.y < position.y)
                    {
                        Debug.DrawLine(new Vector3(e.b.position.x, 0, e.b.position.y), new Vector3(e.a.position.x, 0, e.a.position.y), edgeColor, 100f);
                    }
                }
            }
        }
    }

    public override string ToString()
    {
        return "Partition with pos: "+position.ToString();
    }
}
