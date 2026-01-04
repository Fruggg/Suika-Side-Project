using UnityEngine;

//manages the rendering and lifetime of this thing. violates srp. i dont care dude
public class SplashScript : MonoBehaviour
{
    [SerializeField] Sprite[] possibleSprites;
    [SerializeField] SpriteRenderer spriteRenderer;

    // For the animator to use
    public bool timeToDie = false;
    void Awake()
    {
        int random = Random.Range(0, possibleSprites.Length);
        spriteRenderer.sprite = possibleSprites[random];
    }

    // Update is called once per frame
    void Update()
    {

    if(timeToDie)
        {
            DestroyImmediate(gameObject);
        }
    }
}
