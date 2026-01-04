using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TumblrUser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Image pfp;
    [SerializeField] TextMeshProUGUI saying;

    [SerializeField] private List<string> possiblePosts;
    [SerializeField] List<Image> possiblePFPs;
    void Awake()
    {
        saying.text = possiblePosts[UnityEngine.Random.Range(0, possiblePosts.Count - 1)]; 
        pfp = possiblePFPs[UnityEngine.Random.Range(0, possiblePFPs.Count - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
