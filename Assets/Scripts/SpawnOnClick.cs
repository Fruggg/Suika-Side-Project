using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject prefabToSpawn;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click successful");
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}
