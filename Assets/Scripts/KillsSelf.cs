using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class KillsSelf : MonoBehaviour
{
    
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

}
