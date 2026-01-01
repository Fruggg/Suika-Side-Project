using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private Merge ballData;
    public float cooldownTime = 10.0f;
    public float timeSinceLastSpawn;
    public bool canSpawn = true;

    public bool CheckCooldown() 
    { 
        if (timeSinceLastSpawn >= cooldownTime)
        {
            canSpawn = true;
            
        }
        return canSpawn;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && canSpawn == true)
        {

            CheckCooldown();
            int rn = Random.Range(0, 3);

            var ballInstance = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            canSpawn = false;
            timeSinceLastSpawn = 0f;
            ballInstance.GetComponent<Merge>().SetBallStage(rn);
            
        }
    }
}
