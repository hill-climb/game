using UnityEngine;
using System.Collections;

public class DeadTrigger : MonoBehaviour
{
    [field: SerializeField]
    GameManager manager;

    void Start()
    {
        manager = GameObject.FindObjectOfType<GameManager>();
    }

    public bool enter;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            manager.isDead = true;
            manager.StartDead();
            enter = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            manager.isDead = false;
            enter = false;
        }
    }
}
