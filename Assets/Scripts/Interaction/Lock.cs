using UnityEngine;
using UnityEngine.EventSystems;

public class Lock : MonoBehaviour, IDropHandler, IBeginDragHandler
{
    [SerializeField] private ObjectData unLock; //�ڹ��� Ǯ�� �� ������

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Pointer Down");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Pointer Down");
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("����");
    //    if (collision.gameObject.CompareTag("Key"))
    //    {
    //        GameManager.Instance.AddObject(unLock);
    //        Destroy(collision.gameObject);
    //        Destroy(gameObject);
    //    }
    //}
}
