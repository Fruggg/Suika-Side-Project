using System.Collections;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    private IEnumerator StopTimeCoroutine(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        yield return null;
    }
    public void StopTime(float duration)
    {
        StartCoroutine("StopTimeCoroutine", duration);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
