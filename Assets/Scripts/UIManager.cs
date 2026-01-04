using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nextupText; 
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] SpawnOnClick spawner;

    [SerializeField] int score = 0;
    public void SetScoreAndDisplay(int addedScore)
    {
        score += addedScore;
        scoreText.text = $"score: \n{score}";
    }
    public void SetNextupDisplay(string next)
    {
        
        nextupText.text = $"next: \n{next}";
    }

    private void Update()
    {
        //this is such bad practice
        SetNextupDisplay(spawner.nextToSpawn.ToString());
    }
}
