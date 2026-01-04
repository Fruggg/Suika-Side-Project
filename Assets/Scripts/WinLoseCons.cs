using UnityEngine;

public class WinLoseCons : MonoBehaviour
{
    public bool victory = false;
    public bool defeat = false;
    [SerializeField] UIManager uiManager;
    public int victoryScore = 1000;
    public int maxDeaths = 20;

    public void Victory()
    {
        if (uiManager.score >= victoryScore)
        {
            victory = true;
        }
    }

    public void Defeat()
    {
        if (uiManager.deaths >= maxDeaths)
        {
            defeat = true;
        }
    }

    public void Update()
    {
        Victory();
        Defeat();


    }
}
