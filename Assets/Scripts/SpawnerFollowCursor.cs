using UnityEngine;

public class SpawnerFollowCursor : MonoBehaviour
{

    [SerializeField] float bound1, bound2;
    [SerializeField] bool auto = false;
    [SerializeField] float autonMoveSpeed = 5f;
    int direction = 1;

    void RunAutomatic()
    {

        if (direction > 0 && (transform.position.x >= bound2)) { direction = -1; }
        if (direction < 0 && (transform.position.x <= bound1)) { direction = 1; }
        transform.position += Time.deltaTime * Vector3.right * autonMoveSpeed * direction; // = new Vector2(Mathf.Lerp(transform.position.x, direction.x, moveSpeed), transform.position.y);
        
        
    }
    void Update()
    {
        if (auto) { RunAutomatic(); }
        else
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
}
