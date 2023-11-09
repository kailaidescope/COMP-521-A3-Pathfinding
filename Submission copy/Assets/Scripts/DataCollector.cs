using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    public static int successfulHits = 0;
    public static int totalResets = 0;
    List<float> frameRates = new List<float>();
    const float dataCollectionPeriod = 60f;
    // Never set refreshTime to 0, or this will break
    const float refreshTime = 0.5f;

    int frameCounter = 0;
    float frameTimer = 0f;
    float dataCollectionTimer = 0f;
    bool timeUp = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dataCollectionTimer < dataCollectionPeriod)
        {
            SaveFrameRate();
            dataCollectionTimer += Time.unscaledDeltaTime;
        } else if (!timeUp)
        {
            LogResults();
            timeUp = true;
        }
    }

    void SaveFrameRate()
    {
        if( frameTimer < refreshTime )
        {
            frameTimer += Time.unscaledDeltaTime;
            frameCounter++;
        }
        else
        {
            //Debug.Log(frameCounter/frameTimer);
            frameRates.Add(frameCounter/frameTimer);
            
            frameCounter = 0;
            frameTimer = 0.0f;
        }
    }

    void LogResults()
    {
        float aggregateFrameRate = 0;
        float minFrameRate = -1;
        float maxFrameRate = -1;

        foreach (float f in frameRates)
        {
            aggregateFrameRate += f;

            minFrameRate = (minFrameRate == -1)? f : ((minFrameRate > f)? f : minFrameRate);
            maxFrameRate = (maxFrameRate < f)? f : maxFrameRate;
        }

        frameRates.Sort();

        float medianFrameRate = frameRates[frameRates.Count/2];
        float averageFrameRate = aggregateFrameRate / frameRates.Count;

        Debug.Log("Average frames: "+averageFrameRate+"\nMedian frames: "+medianFrameRate
                +"\nMax frames: "+maxFrameRate+"\nMin frames: "+minFrameRate
                +"\nTotal resets: "+DataCollector.totalResets+"\nSuccessful hits: "+DataCollector.successfulHits);
    }
}
