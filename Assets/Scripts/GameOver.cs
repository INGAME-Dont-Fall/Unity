using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Board"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
