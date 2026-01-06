using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{

    List<Tuple<int, float, float>> timeScaleRequestsInAction = new List<Tuple<int, float, float>>();
    int currentRequestPriority = -1;
    bool currentlyTakingRequest = false;
    private IEnumerator AlterTimeCoroutine(float time, float duration)
    {
        Time.timeScale = time;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        currentlyTakingRequest = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        currentlyTakingRequest = false;
        yield return null;
    }
    public void RequestTimeScale(float timeScale, float duration, int priority = 0)
    {
        Tuple<int, float, float> request = new Tuple<int, float, float>(priority, timeScale, duration);
     
        timeScaleRequestsInAction.Add(request);
    }
    public void StopTime(float duration)
    {
        RequestTimeScale(0, duration);
    }
    public void Update()
    {
        Time.fixedDeltaTime = 0.02f * Time.timeScale;


        if (timeScaleRequestsInAction.Count != 0)
        {
            timeScaleRequestsInAction = timeScaleRequestsInAction.OrderBy(x => x.Item1).ToList();
            // Hold off, the priority is too low
            var request = timeScaleRequestsInAction[0];
            if (currentRequestPriority >= timeScaleRequestsInAction[0].Item1) return;
        

            // Do it
            // Remove the request, buck off the old routine, and begin a new routine with this
            StopCoroutine("AlterTimeCoroutine");
            StartCoroutine(AlterTimeCoroutine(request.Item2, request.Item3));
            timeScaleRequestsInAction.RemoveAt(0);


        }
        else { }
        if (!currentlyTakingRequest) { Time.timeScale = 1; } else { }
        


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
