using UnityEngine;

public class SpawnerFollowCursor : MonoBehaviour
{

    [SerializeField] float bound1, bound2;


    // Update is called once per frame
    void Update()
    {

        Vector2 mousePosition;
        {

            float moveSpeed = 20f;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            float delta = (transform.position.x - mousePosition.x);
            if (delta > 0 && (transform.position.x <= bound1) || (delta < 0 && (transform.position.x >= bound2))) return;
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, mousePosition.x, moveSpeed), transform.position.y);
        }
    }
}
