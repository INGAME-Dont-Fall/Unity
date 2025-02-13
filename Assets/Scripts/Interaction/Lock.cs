using UnityEngine;
using UnityEngine.EventSystems;

public class Lock : MonoBehaviour
{
    [SerializeField] private ObjectData unLock; //자물쇠 풀린 뒤 데이터

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            GameManager.Instance.AddObject(unLock);
            GameManager.Instance.CanvasGroup.blocksRaycasts = true;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
