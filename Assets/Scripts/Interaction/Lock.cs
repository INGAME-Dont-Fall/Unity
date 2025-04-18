using UnityEngine;

public class Lock : Special
{
    [SerializeField] private ObjectData unLock; //자물쇠 풀린 뒤 데이터

    public void GetKey()
    {
        GameManager.Instance.AddObject(unLock, false);
        GameManager.Instance.CanvasGroup.blocksRaycasts = true;
        GameManager.Instance.DecreaseItemsCount();
        Destroy(gameObject);
    }
}
