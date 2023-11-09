using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Human : MonoBehaviour
{
    public static List<Human> humans = new List<Human>();
    public static float speed = 5f;
    public static int recalculateBlockedPathDistance = 1;

    private NavMesh navMesh;
    private Transform target;
    private Vector3 lastTargetPosition;
    private List<Partition> path;

    // Start is called before the first frame update
    void Start()
    {
        humans.Add(this);

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
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateOccupiedSpace();
        HandleBlockedPath();
        MoveToTarget();
        DrawPath();
    }

    // Recalculates path when position of target changes or path is blocked
    void HandleBlockedPath()
    {
        bool blocked = false;

        for (int i = 0; i < Mathf.Min(path.Count, recalculateBlockedPathDistance + 1); i++)
        {
            if (path[i].GetOccupied() != null && path[i].GetOccupied() != gameObject)
            {
                blocked = true;
            }
        }

        if (lastTargetPosition != target.position || blocked)
        {
            path = AStar.FindPath(navMesh.GetPartition(transform.position), navMesh.GetPartition(target.position), gameObject);
            lastTargetPosition = target.position;
        }
    }

    // Handles movement towards target
    void MoveToTarget()
    {
        if (path.Count > 0)
        {
            // Check if the position of the human and the next point on the path are approximately equal.
            if (Vector3.Distance(transform.position, path[0].GetPosition() + Vector3.up.normalized) < 0.001f)
            {
                transform.position = path[0].GetPosition() + Vector3.up.normalized;
                path.RemoveAt(0);
            }

            if (path.Count > 0)
            {
                // Move position a step closer to the target.
                var step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, path[0].GetPosition() + Vector3.up.normalized, step);
            }
        }
    }

    // Draws path in debug window
    void DrawPath()
    {
        for(int i = 0; i < path.Count-1; i++)
        {
            Debug.DrawLine(path[i].GetPosition(), path[i+1].GetPosition(), Color.red);
        }
    }

    public List<Partition> GetPath()
    {
        return path;
    }
}
