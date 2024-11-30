using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Object")
        {
            GameManager.Instance.GameOver();
        }
    }
}
