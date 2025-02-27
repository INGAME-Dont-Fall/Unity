using UnityEngine;
using UnityEngine.EventSystems;

public class Lock : MonoBehaviour
{
    [SerializeField] private ObjectData unLock; //자물쇠 풀린 뒤 데이터

    public void GetKey()
    {
        GameManager.Instance.AddObject(unLock);
        GameManager.Instance.CanvasGroup.blocksRaycasts = true;
        Destroy(gameObject);
    }
}
