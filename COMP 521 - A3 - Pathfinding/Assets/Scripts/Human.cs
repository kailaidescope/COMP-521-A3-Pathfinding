using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Human : MonoBehaviour
{
    public static float speed = 5f;
    private NavMesh navMesh;
    private Transform target;
    private Vector3 lastTargetPosition;
    private List<Partition> path;
    private List<Partition> occupiedSpace;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = FindObjectOfType<NavMesh>();
        var parts = navMesh.GetPartitions();

        int x = Random.Range(0, parts.Count);
        int z = Random.Range(0, parts[x].Count);
        Partition startPartition = parts[x][z];
        transform.position = startPartition.GetPosition() + Vector3.up.normalized;

        /* x = Random.Range(0, parts.Count);
        z = Random.Range(0, parts[x].Count);
        Partition endPartition = parts[x][z];
        path = AStar.FindPath(startPartition, endPartition); */

        target = navMesh.endTransform;
        lastTargetPosition = target.position;
        path = AStar.FindPath(navMesh.GetPartition(transform.position), navMesh.GetPartition(target.position));

        occupiedSpace = new List<Partition>();
        occupiedSpace.Add(startPartition);
    }

    // Update is called once per frame
    void Update()
    {
        // Update what space is taken up by this object
        List<Partition> newOccupied = new List<Partition>();

        newOccupied.Add(navMesh.GetPartition(transform.position).SetOccupied(gameObject));

        if(path.Count > 0 && path[0] != navMesh.GetPartition(transform.position)) { newOccupied.Add(path[0].SetOccupied(gameObject)); }
        
        foreach (Partition p in occupiedSpace)
        {
            if(!newOccupied.Contains(p))
            {
                p.SetOccupied(null);
            }
        }

        occupiedSpace = newOccupied;

        /* foreach (Partition p in occupiedSpace)
        {
            p.Draw(Color.yellow);
        } */

        // Recalculates path when position of target changes or path is blocked
        bool blocked = false;

        foreach (Partition p in path)
        {
            if (p.GetOccupied() != null && p.GetOccupied() != gameObject)
            {
                blocked = true;
            }
        }

        if (lastTargetPosition != target.position || blocked)
        {
            path = AStar.FindPath(navMesh.GetPartition(transform.position), navMesh.GetPartition(target.position), gameObject);
            lastTargetPosition = target.position;
        }

        // Handles movement towards target
        if (path.Count > 0)
        {
            // Check if the position of the human and target are approximately equal.
            if (Vector3.Distance(transform.position, path[0].GetPosition() + Vector3.up.normalized) < 0.001f)
            {
                transform.position = path[0].GetPosition() + Vector3.up.normalized;
                path.RemoveAt(0);
            }

            if (path.Count > 0)
            {
                // Move our position a step closer to the target.
            var step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, path[0].GetPosition() + Vector3.up.normalized, step);
            }
        }
        DrawPath();
    }

    // Draws path in debug window
    void DrawPath()
    {
        for(int i = 0; i < path.Count-1; i++)
        {
            Debug.DrawLine(path[i].GetPosition(), path[i+1].GetPosition(), Color.red);
        }
    }
}
