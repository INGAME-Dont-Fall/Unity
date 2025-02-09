using UnityEngine;
using UnityEngine.EventSystems;

public class Lock : MonoBehaviour, IDropHandler, IBeginDragHandler
{
    [SerializeField] private ObjectData unLock; //자물쇠 풀린 뒤 데이터

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("응애");
    //    if (collision.gameObject.CompareTag("Key"))
    //    {
    //        GameManager.Instance.AddObject(unLock);
    //        Destroy(collision.gameObject);
    //        Destroy(gameObject);
    //    }
    //}
}
