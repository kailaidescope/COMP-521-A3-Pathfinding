using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    NavMesh navMesh;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = FindObjectOfType<NavMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Mover"))
        {
            float minx = Mathf.Min(navMesh.referencePlaneCorners[0].x, navMesh.referencePlaneCorners[1].x) + 0.5f;
            float maxx = Mathf.Max(navMesh.referencePlaneCorners[0].x, navMesh.referencePlaneCorners[1].x) - 0.5f;
            float minz = Mathf.Min(navMesh.referencePlaneCorners[0].y, navMesh.referencePlaneCorners[1].y) + 0.5f;
            float maxz = Mathf.Max(navMesh.referencePlaneCorners[0].y, navMesh.referencePlaneCorners[1].y) - 0.5f;

            transform.position = new Vector3(Random.Range(minx, maxx), 0.5f, Random.Range(minz,maxz));  
        }
    }
}