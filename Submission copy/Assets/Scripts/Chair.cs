using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public static List<Chair> chairs = new List<Chair>();
    public static float speed = Human.speed/2;
    public static int recalculateBlockedPathDistance = 1;


    private Human targetHuman;
    private Partition targetPartition;
    private NavMesh navMesh;
    private List<Partition> path;

    // Start is called before the first frame update
    void Start()
    {
        chairs.Add(this);

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
    }

    // Update is called once per frame
    void Update()
    {
        HandleFindTarget();
        HandlePathRecalculation();
        MoveToTarget();
        DrawPath();
    }

    // Selects a random human to block and the part of there path to block them on
    void HandleFindTarget()
    {
        if (targetHuman == null)
        {
            targetHuman = Human.humans[Random.Range(0,Human.humans.Count)];
        }

        if (targetPartition == null)
        {
            if (targetHuman.GetPath() != null)
            {
                int idealTargetDepth = Mathf.FloorToInt((targetHuman.transform.position - transform.position).magnitude);
                int targetDepth = Mathf.Min(idealTargetDepth, targetHuman.GetPath().Count-1);

                // Gets partition closest to the targetPartitionDepth
                targetPartition = targetHuman.GetPath()[targetDepth];

                // Stops chairs from clumping on target
                if (targetPartition == navMesh.GetPartition(Target.target.transform.position) && targetDepth != 0)
                {
                    targetDepth--;
                    targetPartition = targetHuman.GetPath()[targetDepth];
                }

                path = null;
            }
        } else 
        {
            if (targetHuman.GetPath() != null && !targetHuman.GetPath().Contains(targetPartition))
            {
                int idealTargetDepth = Mathf.FloorToInt((targetHuman.transform.position - transform.position).magnitude);
                int targetDepth = Mathf.Min(idealTargetDepth, targetHuman.GetPath().Count-1);

                // Gets partition closest to the targetPartitionDepth
                targetPartition = targetHuman.GetPath()[targetDepth];

                // Stops chairs from clumping on target
                if (targetPartition == navMesh.GetPartition(Target.target.transform.position) && targetDepth != 0)
                {
                    targetDepth--;
                    targetPartition = targetHuman.GetPath()[targetDepth];
                }

                path = null;
            }
        }
    }

    // Recalculates path when blocked or destination changed
    void HandlePathRecalculation()
    {
        bool blocked = false;
        if (path != null)
        {
            for (int i = 0; i < Mathf.Min(path.Count, recalculateBlockedPathDistance); i++)
            {
                if (path[i].GetOccupied() != null && path[i].GetOccupied() != gameObject)
                {
                    blocked = true;
                }
            }
        }

        if (path == null || blocked)
        {
            path = AStar.FindPath(navMesh.GetPartition(transform.position), targetPartition, gameObject);
        }
    }

    // Handles movement towards target
    void MoveToTarget()
    {
        if (path != null && path.Count > 0)
        {
            // Check if the position of the chair and next point on the path are approximately equal.
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
        if (path != null) 
        {
            for(int i = 0; i < path.Count-1; i++)
            {
                Debug.DrawLine(path[i].GetPosition(), path[i+1].GetPosition(), Color.red);
            }
        }
    }

    public void ResetTargetHuman()
    {
        targetHuman = null;
    }
}
