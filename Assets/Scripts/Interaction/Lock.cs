using UnityEngine;
using UnityEngine.EventSystems;

public class Lock : Special
{
    [SerializeField] private ObjectData unLock; //�ڹ��� Ǯ�� �� ������

    public void GetKey()
    {
        GameManager.Instance.AddObject(unLock, false);
        GameManager.Instance.CanvasGroup.blocksRaycasts = true;
        GameManager.Instance.DecreaseItemsCount();
        Destroy(gameObject);
    }
}
