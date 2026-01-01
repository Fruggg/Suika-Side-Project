using UnityEngine;

public class loopingMover : MonoBehaviour
{

    // Core idea here is to apply a constant movement
    public Vector3 offset;
    public float bonusOffset;
    float accumulator = 0;
    [SerializeField] float cap = 1; 
    [SerializeField] float stepVal;

    private void Awake()
    {
        offset = transform.localPosition - Vector3.left * bonusOffset;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("balls");
        }

        accumulator += Time.deltaTime * stepVal;
        transform.localPosition = accumulator * new Vector3(1,-1,0) - offset;
        if (accumulator > cap)
        {
            accumulator = 0;
        }
    }
}
