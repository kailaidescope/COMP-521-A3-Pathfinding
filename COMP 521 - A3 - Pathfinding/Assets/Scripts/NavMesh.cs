using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class NavMesh : MonoBehaviour
{
    [SerializeField] Vector2[] referencePlaneCorners = new Vector2[2];
    [SerializeField] float height;

    private List<List<Partition>> partitions;
    float xStorageOffset;
    float yStorageOffset;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNavMesh();

        foreach (List<Partition> ps in partitions)
        {
            foreach (Partition p in ps)
            {
                p.Draw(Color.yellow, Color.red);
            }
        }

    }

    void GenerateNavMesh()
    {
        partitions = new List<List<Partition>>();

        float minx = Mathf.Min(referencePlaneCorners[0].x, referencePlaneCorners[1].x);
        float maxx = Mathf.Max(referencePlaneCorners[0].x, referencePlaneCorners[1].x) - 1;
        float miny = Mathf.Min(referencePlaneCorners[0].y, referencePlaneCorners[1].y);
        float maxy = Mathf.Max(referencePlaneCorners[0].y, referencePlaneCorners[1].y) - 1;

        xStorageOffset = -minx - 0.5f;
        yStorageOffset = -miny - 0.5f;

        for (float x = minx; x <= maxx; x++)
        {
            partitions.Add(new List<Partition>());

            for (float y = miny; y <= maxy; y++)
            {
                //Debug.Log("("+x+","+y+")");
                Partition p = new Partition(new Vector2(x+0.5f, y+0.5f));

                partitions[(int)(p.GetPosition().x + xStorageOffset)].Add(p);

/*                 int storagex = (int)(p.GetPosition().x + xStorageOffset);
                int storagey = (int)(p.GetPosition().y + yStorageOffset);

                Debug.Log("[" + storagex + ", " + storagey
                            + "]: " + partitions[storagex][storagey]); */
                
                // Add edges to existing nodes
                if (x > minx)
                {
                    // Connect to left node
                    AddEdge(p, GetPartition(x-0.5f, y+0.5f));

                    if (y < maxy)
                    {
                        // Connect to upper-left node
                        AddEdge(p, GetPartition(x-0.5f, y+1.5f));
                    }
                    if(y > miny)
                    {
                        // Connect to lower-left node
                        AddEdge(p, GetPartition(x-0.5f, y-0.5f));
                    }
                }
                if (y > miny)
                {
                    // Connect to lower node
                    AddEdge(p, GetPartition(x+0.5f,y-0.5f));
                }
            }
        }
    }

    // Create an edge between two nodes in the navmesh
    void AddEdge(Partition a, Partition b)
    {
        Edge edge = new Edge(a,b);
        a.GetEdges().Add(edge);
        b.GetEdges().Add(edge);
    }

    Partition GetPartition(float x, float y)
    {
        //Debug.Log("x = "+(int)(x+xStorageOffset)+", y = "+(int)(y+yStorageOffset));
        return partitions[(int)(x+xStorageOffset)][(int)(y+yStorageOffset)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        float[] xs = {referencePlaneCorners[0].x, referencePlaneCorners[1].x};
        float[] ys = {referencePlaneCorners[0].y, referencePlaneCorners[1].y};
        
        Gizmos.DrawLine(new Vector3(xs[0], height, ys[0]), new Vector3(xs[0], height, ys[1]));
        Gizmos.DrawLine(new Vector3(xs[0], height, ys[1]), new Vector3(xs[1], height, ys[1]));
        Gizmos.DrawLine(new Vector3(xs[1], height, ys[1]), new Vector3(xs[1], height, ys[0]));
        Gizmos.DrawLine(new Vector3(xs[1], height, ys[0]), new Vector3(xs[0], height, ys[0]));
    }
}
