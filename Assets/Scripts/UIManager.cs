using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nextupText; 
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] SpawnOnClick spawner;

    public int score = 0;
    public int deaths = 0;
    public void SetScoreAndDisplay(int addedScore)
    {
        score += addedScore;
        scoreText.text = $"score: \n{score}";
    }
    public void SetNextupDisplay(string next)
    {
        
        nextupText.text = $"next: \n{next}";
    }

    public void SetDeaths(int deaths) 
    { 
        Debug.Log("Death registered in UIManager");
        deaths += 1;
        Debug.Log($"Total Deaths: {deaths}");

    }

    private void Update()
    {
        //this is such bad practice
        SetNextupDisplay(spawner.nextToSpawn.ToString());
    }
}
