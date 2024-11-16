using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int index = 0;
    private CanvasGroup canvasGroup;
    private GameObject currentDraggedObject;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponentInParent<CanvasGroup>();
    }

    //드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        //부모 오브젝트는 인벤토리의 빈칸이 됨
        GameObject go = transform.parent.gameObject;
        GameManager.Instance.emptyInventory.Add(go);
        canvasGroup.blocksRaycasts = false;

        //이미지는 끄고 정해진 오브젝트를 생성
        gameObject.GetComponent<Image>().enabled = false;
        currentDraggedObject = Instantiate(GameManager.Instance.GamePrefab[index].gameObj, GameManager.Instance.objectGroup.transform);
        currentDraggedObject.GetComponent<DragObj>().index = index;
    }

    //드래그 중 계속 호출
    public void OnDrag(PointerEventData eventData)
    {
        // 드래그한 오브젝트가 마우스를 따라 움직이게 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        currentDraggedObject.transform.position = mousePos;
    }

    //드래그 끝날 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(gameObject);
        canvasGroup.blocksRaycasts = true;
    }
}
