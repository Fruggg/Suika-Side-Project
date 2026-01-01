using UnityEngine;

public class loopingMover : MonoBehaviour
{

    // Core idea here is to apply a constant movement
    public Vector3 offset;
    float accumulator = 0;
    [SerializeField] float cap = 1; 
    [SerializeField] float stepVal;

    private void Awake()
    {
        offset = transform.position;
    }
    void Update()
    {

        accumulator += Time.deltaTime * stepVal;
        transform.localPosition = accumulator * new Vector3(1,-1,0) - offset;
        if (accumulator > cap)
        {
            accumulator = 0;
        }
    }
}
