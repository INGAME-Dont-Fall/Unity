using UnityEngine;
using UnityEngine.EventSystems;

public class Lock : MonoBehaviour
{
    [SerializeField] private ObjectData unLock; //�ڹ��� Ǯ�� �� ������

    public void GetKey()
    {
        GameManager.Instance.AddObject(unLock);
        GameManager.Instance.CanvasGroup.blocksRaycasts = true;
        Destroy(gameObject);
    }
}
