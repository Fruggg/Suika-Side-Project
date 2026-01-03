using UnityEngine;

public class FlashEnabler : MonoBehaviour
{
    [SerializeField] Animator flashAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] bool cachedFlashStatus = false;
    public bool enableFlash;

    // Update is called once per frame
    void Update()
    {
        //if (enableFlash != cachedFlashStatus)
        //{
        //    flashAnimator.SetTrigger("Flash");
        //}
        //else { }
        //cachedFlashStatus = enableFlash;
    }
}
