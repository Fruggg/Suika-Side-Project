using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Merge;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private Merge ballData;
    public float cooldownTime = 10.0f;
    public float timeSinceLastSpawn;
    public bool canSpawn = true;
    [SerializeField] bool auto = false; // If it runs automatically or not
    [SerializeField] UIManager uiManager;
    [SerializeField] int spawningRange = 3;

    // public access for UI - this is bad.
    public BallType nextToSpawn = BallType.Wheatley;
    private bool WantsToSpawn()
    {
        // Always send it during auto
        if (auto) { return true; }

        // If mot auto it does this
        return Input.GetMouseButtonDown(0);
    }

    // Public because we're gonna need it for our rendering class
    public BallType NextBall()
     {
        return nextToSpawn;
     }
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= cooldownTime) { canSpawn = true; }
        if (WantsToSpawn() && canSpawn == true)
        {
            int rn = (int)NextBall(); 

            var ballInstance = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            Merge instanceBallScript = ballInstance.GetComponent<Merge>();
            canSpawn = false;
            timeSinceLastSpawn = 0f;
            instanceBallScript.SetBallStage(rn);
            instanceBallScript.OnMerge.AddListener(uiManager.SetScoreAndDisplay);
            instanceBallScript.OnDeath.AddListener(uiManager.SetDeaths);
            nextToSpawn = (BallType)Random.Range(0, spawningRange);
        }
    }
}
