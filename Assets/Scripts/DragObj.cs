using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class DragObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index = 0;
    private Canvas canvas;  // 오브젝트를 드랍할 UI 캔버스
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvas = GameManager.Instance.Canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
    }

    //드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스를 따라오도록 오브젝트 위치 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, gameObject.transform.position.z);
    }

    // 드래그 종료 시 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (GameObject go in GameManager.Instance.EmptyInventory)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(go.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), canvas.worldCamera))
            {
                // UI 프리팹을 생성하고 해당 UI에 종속시킴
                GameObject newUIObject = Instantiate(GameManager.Instance.GamePrefab[index].gameUI, go.transform);
                newUIObject.GetComponent<DragUI>().index = index;

                //칸이 찼으니까 삭제
                //GameManager.Instance.EmptyInventory.Remove(go);
                Destroy(gameObject);
            }
        }

        canvasGroup.blocksRaycasts = true;
    }
}
