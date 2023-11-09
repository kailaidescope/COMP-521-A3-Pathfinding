using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static Target target;
    public static float waitBetweenResetTime = 0.5f;
    public static float resetBoardTime = 10f;

    float timer;
    NavMesh navMesh;

    // Start is called before the first frame update
    void Start()
    {
        target = this;
        timer = 0f;
        navMesh = FindObjectOfType<NavMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= resetBoardTime)
        {
            ResetPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Human"))
        {
            ResetPosition();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Human"))
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        StartCoroutine(Wait());
        timer = 0f;

        // Reset the targets of all chairs
        foreach (Chair c in Chair.chairs)
        {
            c.ResetTargetHuman();
        }

        float minx = Mathf.Min(navMesh.referencePlaneCorners[0].x, navMesh.referencePlaneCorners[1].x) + 0.5f;
        float maxx = Mathf.Max(navMesh.referencePlaneCorners[0].x, navMesh.referencePlaneCorners[1].x) - 0.5f;
        float minz = Mathf.Min(navMesh.referencePlaneCorners[0].y, navMesh.referencePlaneCorners[1].y) + 0.5f;
        float maxz = Mathf.Max(navMesh.referencePlaneCorners[0].y, navMesh.referencePlaneCorners[1].y) - 0.5f;

        transform.position = new Vector3(Random.Range(minx, maxx), 0.5f, Random.Range(minz,maxz));  
    }

    IEnumerator Wait()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitBetweenResetTime);
        Time.timeScale = 1;
    }
}