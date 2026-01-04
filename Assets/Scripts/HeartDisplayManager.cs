using UnityEngine;

public class HeartDisplayManager : MonoBehaviour
{

    [SerializeField] GameObject heartPrefab;
    public void SetCount(int amount)
    {
        while(transform.childCount < amount)
        {
            Instantiate(heartPrefab, transform);        
        }
        if (transform.childCount > amount)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
